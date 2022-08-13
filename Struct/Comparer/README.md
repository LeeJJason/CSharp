# C# 中 struct 比较的注意事项

声明结构体

```cs
public struct Struct : System.IEquatable<Struct>
{
    public int a;
}
```

在泛型中使用该结构

```CS
Struct s = new Struct() { a = 10 };
List<Struct> list = new List<Struct>() { s };
Dictionary<Struct, Struct> dict = new Dictionary<Struct, Struct>() { { s, s } 
```

使用容器的查找相关操作会造成GC

```CS
list.Contains(s);  //装箱40b
dict.ContainsKey(s);
dict.ContainsValue(s);
var n = dict[s];
```

* 在 Dictionary 中当作 Key 使用时，会调用 GetHashCode 方法获取对象 Hash 判断对象是否相同，对象会被装箱为 object 对象。

    ```cs
    //object 中的 GetHashCode 实现
    public virtual int GetHashCode()
    {
    	return RuntimeHelpers.GetHashCode(this);
    }
    ```

* 对象判断相等时调用 Equals 相关方法，会有装箱操作，需要重写

    ```cs
    public virtual bool Equals(object obj)
    public static bool Equals(object objA, object objB)
    ```

* 容器内部判断相等使用 `EqualityComparer<T>.Default` 检测，需要自定义 `EqualityComparer` 类

    ```cs
    //EqualityComparer<T>.Defaul 会检测 T 有没有实现 IEquatable < T > 接口
    //若实现则使用 自定义的 `EqualityComparer` 类，否则使用Object默认的类
    public struct Struct : System.IEquatable<Struct>
    {
    }
    
    public class StructEqualityComparer : EqualityComparer<Struct>
    {
        public override bool Equals(Struct x, Struct y)
        {
            return x == y;
        }
    
        public override int GetHashCode(Struct obj)
        {
            return obj.GetHashCode();
        }
    }
    ```

    

## 完整实现

```cs
public struct Struct : System.IEquatable<Struct>
{
    public int a;
    public static bool Equals(Struct a, Struct b) 
    {
        return a.Equals(b);
    }
    public bool Equals(Struct other) 
    { 
        return a == other.a; 
    }
    public static bool operator ==(Struct a, Struct b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(Struct a, Struct b)
    {
        return !a.Equals(b);
    }
    public override int GetHashCode() 
    { 
        return a; 
    }
}

public class StructEqualityComparer : EqualityComparer<Struct>
{
    public override bool Equals(Struct x, Struct y)
    {
        return x == y;
    }

    public override int GetHashCode(Struct obj)
    {
        return obj.GetHashCode();
    }
}
```

有效避免 Struct 类型在容器中使用时产生GC。