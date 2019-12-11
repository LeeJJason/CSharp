
<!-- TOC -->

- [自定义结构体作为hash容器 key 的解决方案](#自定义结构体作为hash容器-key-的解决方案)
    - [C# 代码](#c-代码)
        - [InfoStruct 代码](#infostruct-代码)
        - [InfoClass 代码](#infoclass-代码)
        - [执行 代码](#执行-代码)
    - [代码分析](#代码分析)
        - [问题](#问题)
        - [分析](#分析)
            - [容器 `Add` 函数分析：](#容器-add-函数分析)
            - [泛型的缺省的比较类型分析：](#泛型的缺省的比较类型分析)
            - [装箱代码分析](#装箱代码分析)
                - [C# 代码](#c-代码-1)
                - [IL 代码](#il-代码)
    - [解决方案](#解决方案)

<!-- /TOC -->
# 自定义结构体作为hash容器 key 的解决方案
## C# 代码 
### InfoStruct 代码
```CSharp
public struct InfoStruct
{
    public int x;

    public InfoStruct(int x)
    {
        this.x = x;
    }

    public override int GetHashCode()
    {
        return x;
    }

    public bool Equals(InfoStruct infoStruct)
    {
        return this.x == infoStruct.x;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((InfoStruct)obj);
    }

    public static bool operator ==(InfoStruct a, InfoStruct b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(InfoStruct a, InfoStruct b)
    {
        return !(a == b);
    }
}
```
### InfoClass 代码
```CSharp
public class InfoClass
{
    public int x;
    public InfoClass(int x)
    {
        this.x = x;
    }

    public override int GetHashCode()
    {
        return this.x;
    }

    public bool Equals(InfoClass obj)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(null, obj)) return false;
        return this.x == obj.x;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((InfoClass)obj);
    }

    public static bool operator ==(InfoClass a, InfoClass b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(null, a)) return false;
        return a.Equals(b);
    }

    public static bool operator !=(InfoClass a, InfoClass b)
    {
        return !(a == b);
    }
}
```
### 执行 代码
```CSharp
public static void FunClass()
{
    InfoClass infoClass = new InfoClass(1);
    InfoClass infoClass1 = new InfoClass(1);
    InfoClass infoClass2 = new InfoClass(2);

    dictc.Add(infoClass, 2);
    // 1、 Add操作
    // System.ArgumentException:“已添加了具有相同键的项。”
    //dictc.Add(infoClass1, 3);

    // 2、 [] 操作
    dictc[infoClass1] = 3;
    System.Console.WriteLine("infoClass => " + dictc[infoClass]);
    System.Console.WriteLine("infoClass1 => " + dictc[infoClass1]);
    System.Console.WriteLine("infoClass1 == infoClass2 => " + (infoClass2 == infoClass1));
}

public static void FunStruct()
{
    InfoStruct infoStruct = new InfoStruct(1);
    InfoStruct infoStruct1 = new InfoStruct(2);
    InfoStruct infoStruct2 = new InfoStruct(3);

    dicts.Add(infoStruct, 2);
    // 1、 Add操作
    dicts.Add(infoStruct1, 3);
    // 2、 [] 操作
    dicts[infoStruct2] = 3;
    System.Console.WriteLine("infoStruct => " + dicts[infoStruct]);
    System.Console.WriteLine("infoStruct1 => " + dicts[infoStruct1]);
    System.Console.WriteLine("infoStruct1 == infoStruct2 => " + (infoStruct2 == infoStruct1));
}
```

## 代码分析
先调用 `int GetHashCode()`，在两个对象返回值相同时，调用 `bool Equals(object obj)`，返回 `true` 时，容器认为两个对象相等。

### 问题
* *值类型作为 key 时会不会调用 `bool Equals(object obj)`?*
* *值类型作为 key 时会不会产生装箱拆箱操作?*

### 分析
#### 容器 `Add` 函数分析：

```CSharp
private void Insert(TKey key, TValue value, bool add)
{
    if (key == null)
    {
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
    }
    if (buckets == null)
    {
        Initialize(0);
    }
    int num = comparer.GetHashCode(key) & int.MaxValue;
    int num2 = num % buckets.Length;
    int num3 = 0;
    for (int num4 = buckets[num2]; num4 >= 0; num4 = entries[num4].next)
    {
        if (entries[num4].hashCode == num && comparer.Equals(entries[num4].key, key))
        {
            if (add)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
            }
            entries[num4].value = value;
            version++;
            return;
        }
        num3++;
    }
    int num5;
    if (freeCount > 0)
    {
        num5 = freeList;
        freeList = entries[num5].next;
        freeCount--;
    }
    else
    {
        if (count == entries.Length)
        {
            Resize();
            num2 = num % buckets.Length;
        }
        num5 = count;
        count++;
    }
    entries[num5].hashCode = num;
    entries[num5].next = buckets[num2];
    entries[num5].key = key;
    entries[num5].value = value;
    buckets[num2] = num5;
    version++;
    if (num3 > 100 && HashHelpers.IsWellKnownEqualityComparer(comparer))
    {
        comparer = (IEqualityComparer<TKey>)HashHelpers.GetRandomizedEqualityComparer(comparer);
        Resize(entries.Length, true);
    }
}
```
过程中使用了`comparer`字段来调用 `Equals` 和 `GetHashCode` 方法。`comparer`为`IEqualityComparer<TKey>`类型。该类型在构建容器对象时通过通过构造函数传入，否则使用泛型的缺省的比较类型。

#### 泛型的缺省的比较类型分析：
``` CSHARP
internal class ObjectEqualityComparer<T> : EqualityComparer<T>
{
    public override bool Equals(T x, T y)
    {
        if (x != null)
        {
            if (y != null)
            {
                return x.Equals(y);
            }
            return false;
        }
        if (y != null)
        {
            return false;
        }
        return true;
    }

    public override int GetHashCode(T obj)
    {
        return obj?.GetHashCode() ?? 0;
    }

    internal override int IndexOf(T[] array, T value, int startIndex, int count)
    {
        int num = startIndex + count;
        if (value == null)
        {
            for (int i = startIndex; i < num; i++)
            {
                if (array[i] == null)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int j = startIndex; j < num; j++)
            {
                if (array[j] != null && array[j].Equals(value))
                {
                    return j;
                }
            }
        }
        return -1;
    }

    internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
    {
        int num = startIndex - count + 1;
        if (value == null)
        {
            for (int num2 = startIndex; num2 >= num; num2--)
            {
                if (array[num2] == null)
                {
                    return num2;
                }
            }
        }
        else
        {
            for (int num3 = startIndex; num3 >= num; num3--)
            {
                if (array[num3] != null && array[num3].Equals(value))
                {
                    return num3;
                }
            }
        }
        return -1;
    }

    public override bool Equals(object obj)
    {
        ObjectEqualityComparer<T> objectEqualityComparer = obj as ObjectEqualityComparer<T>;
        return objectEqualityComparer != null;
    }

    public override int GetHashCode()
    {
        return GetType().Name.GetHashCode();
    }
}

```
`bool Equals(T x, T y)` IL 代码
```CSHARP
.method public hidebysig virtual instance bool Equals (!T x, !T y) cil managed 
{
    // Method begins at RVA 0xda90a
    // Code size 50 (0x32)
    .maxstack 8

    IL_0000: ldarg.1
    IL_0001: box !T
    IL_0006: brfalse.s IL_0026

    IL_0008: ldarg.2
    IL_0009: box !T
    IL_000e: brfalse.s IL_0024

    IL_0010: ldarga.s x
    IL_0012: ldarg.2
    IL_0013: box !T
    IL_0018: constrained. !T
    IL_001e: callvirt instance bool System.Object::Equals(object)
    IL_0023: ret

    IL_0024: ldc.i4.0
    IL_0025: ret

    IL_0026: ldarg.2
    IL_0027: box !T
    IL_002c: brfalse.s IL_0030

    IL_002e: ldc.i4.0
    IL_002f: ret

    IL_0030: ldc.i4.1
    IL_0031: ret
} // end of method ObjectEqualityComparer`1::Equals
```
在比较过程中，对两个泛型对象进行了两次装箱，最后调用 `System.Object` 上的 `Equals` 方法进行比较。
#### 装箱代码分析
*是否在判断为null的时候产生？*
测试代码
##### C# 代码
```CS
public static void GenericStruct<T>(T info)
{
    if(info == null)
    {
        Console.WriteLine("Box Test");
    }
}
```
##### IL 代码
```CS
.method public hidebysig static void  GenericStruct<T>(!!T info) cil managed
{
  // Code size       28 (0x1c)
  .maxstack  2
  .locals init ([0] bool V_0)
  IL_0000:  nop
  IL_0001:  ldarg.0
  IL_0002:  box        !!T
  IL_0007:  ldnull
  IL_0008:  ceq
  IL_000a:  stloc.0
  IL_000b:  ldloc.0
  IL_000c:  brfalse.s  IL_001b
  IL_000e:  nop
  IL_000f:  ldstr      "Box Test"
  IL_0014:  call       void [mscorlib]System.Console::WriteLine(string)
  IL_0019:  nop
  IL_001a:  nop
  IL_001b:  ret
} // end of method Program::GenericStruct
```

确定泛型判空会发生装箱操作，因为值类型不会为空。

## 解决方案
实现自定义的 `EqualityComparer` 类：
```CS
public class ValueEqualityComparer : EqualityComparer<InfoStruct>
{
    public override bool Equals(InfoStruct x, InfoStruct y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode(InfoStruct obj)
    {
        return obj.GetHashCode();
    }
}
```
`Equals` IL 代码
```CS
.method public hidebysig virtual instance bool 
        Equals(valuetype HashContainer.InfoStruct x,
               valuetype HashContainer.InfoStruct y) cil managed
{
  // Code size       14 (0xe)
  .maxstack  2
  .locals init ([0] bool V_0)
  IL_0000:  nop
  IL_0001:  ldarga.s   x
  IL_0003:  ldarg.2
  IL_0004:  call       instance bool HashContainer.InfoStruct::Equals(valuetype HashContainer.InfoStruct)
  IL_0009:  stloc.0
  IL_000a:  br.s       IL_000c
  IL_000c:  ldloc.0
  IL_000d:  ret
} // end of method ValueEqualityComparer::Equals

```