# BindingFlags 

## 1、InvokeMember
### 1.1、InvokeMember(String, BindingFlags, Binder, Object, Object[], ParameterModifier[], CultureInfo, String[])

```cs
public abstract object InvokeMember (string name, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object target, object[] args, System.Reflection.ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters);
```
**Parameters**
1. **string name**  
    它包含要调用的构造函数、方法、属性或字段成员的名称。或 空字符串 ("")，表示调用默认成员。或 对于 IDispatch 成员，则为一个表示 DispID 的字符串，例如"[DispID=3]"。
2. **BindingFlags invokeAttr**  
    按位组合的枚举值，这些值指定如何进行搜索。 访问可以是 BindingFlags 之一，如 Public、NonPublic、Private、InvokeMethod 和 GetField 等。 查找类型无需指定。 如果省略查找的类型，则将使用 BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static。
3. **Binder binder**  
    一个对象定义一组属性并启用绑定，而绑定可能涉及选择重载方法、强制参数类型和通过反射调用成员。或 空引用（在 Visual Basic 中为 Nothing）要使用 DefaultBinder。 请注意，为了成功地使用变量参数来调用方法重载，可能必须显式定义 Binder 对象。
4. **object target**  
    对其调用指定成员的对象。
5. **object[] args**  
    包含传递给要调用的成员的参数的数组。
6. **ParameterModifier[] modifiers**  
    ParameterModifier 对象的数组，表示与 args 数组中的相应元素关联的特性。 参数的关联的属性存储在成员的签名中。只有在调用 COM 组件时，默认联编程序才处理此参数。
7. **CultureInfo culture**  
    表示要使用的全局化区域设置的 CultureInfo 对象，它对区域设置特定的转换可能是必需的，比如将数字 String 转换为 Double。或指定当前线程的CultureInfo为null（在 Visual Basic 中为 Nothing）。
8. **string[] namedParameters**
    包含参数名称的数组，args 数组中的值将传递给这些参数。

**Return Object**  
一个对象，表示被调用成员的返回值。

---

### 1.2、InvokeMember(String, BindingFlags, Binder, Object, Object[])
```cs
public object InvokeMember (string name, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object target, object[] args);
```
**Parameters**
1. **string name**  
    它包含要调用的构造函数、方法、属性或字段成员的名称。或 空字符串 ("")，表示调用默认成员。或 对于 IDispatch 成员，则为一个表示 DispID 的字符串，例如"[DispID=3]"。
2. **BindingFlags invokeAttr**  
    按位组合的枚举值，这些值指定如何进行搜索。 访问可以是 BindingFlags 之一，如 Public、NonPublic、Private、InvokeMethod 和 GetField 等。 查找类型无需指定。 如果省略查找的类型，则将使用 BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static。
3. **Binder binder**  
    一个对象定义一组属性并启用绑定，而绑定可能涉及选择重载方法、强制参数类型和通过反射调用成员。或 空引用（在 Visual Basic 中为 Nothing）要使用 DefaultBinder。 请注意，为了成功地使用变量参数来调用方法重载，可能必须显式定义 Binder 对象。
4. **object target**  
    对其调用指定成员的对象。
5. **object[] args**  
    包含传递给要调用的成员的参数的数组。

**Return Object**  
一个对象，表示被调用成员的返回值。

---

### 1.3 InvokeMember(String, BindingFlags, Binder, Object, Object[], CultureInfo)
```cs
public object InvokeMember (string name, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, object target, object[] args, System.Globalization.CultureInfo culture);
```
**Parameters**
1. **string name**  
    它包含要调用的构造函数、方法、属性或字段成员的名称。或 空字符串 ("")，表示调用默认成员。或 对于 IDispatch 成员，则为一个表示 DispID 的字符串，例如"[DispID=3]"。
2. **BindingFlags invokeAttr**  
    按位组合的枚举值，这些值指定如何进行搜索。 访问可以是 BindingFlags 之一，如 Public、NonPublic、Private、InvokeMethod 和 GetField 等。 查找类型无需指定。 如果省略查找的类型，则将使用 BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static。
3. **Binder binder**  
    一个对象定义一组属性并启用绑定，而绑定可能涉及选择重载方法、强制参数类型和通过反射调用成员。或 空引用（在 Visual Basic 中为 Nothing）要使用 DefaultBinder。 请注意，为了成功地使用变量参数来调用方法重载，可能必须显式定义 Binder 对象。
4. **object target**  
    对其调用指定成员的对象。
5. **object[] args**  
    包含传递给要调用的成员的参数的数组。
6. **CultureInfo culture**
    表示要使用的全局化区域设置的 CultureInfo 对象，它对区域设置特定的转换可能是必需的，比如将数字 String 转换为 Double。或指定当前线程的CultureInfo为null（在 Visual Basic 中为 Nothing）。  

**Return Object**  
    一个对象，表示被调用成员的返回值。

---

## 2、字段
|字段|值|描述|
|:---|:---|:---|
|CreateInstance|512|指定反射应创建指定类型的实例。 调用与给定参数匹配的构造函数。 忽略提供的成员名称。 如果未指定查找的类型，则应用“（Instance &#124; Public）”。 不能调用类型初始值设定项。此标志会传递给 **InvokeMember** 方法以调用构造函数。
|DeclaredOnly|2|指定只应考虑在所提供类型的层次结构级别上声明的成员。 不考虑继承的成员。|
|Default|0|指定未定义任何绑定标志。|
|ExactBinding|65536|指定提供的参数的类型必须与对应形参的类型完全匹配。 如果调用方提供非 null Binder 对象，则反射会引发异常，因为这意味着调用方在提供将选取适当方法的 BindToXXX 实现。 当自定义绑定器可实现此标志的语义时，默认绑定器会忽略此标志。|
|FlattenHierarchy|64|指定应返回层次结构往上的公共成员和受保护静态成员。 不返回继承类中的私有静态成员。 静态成员包括字段、方法、事件和属性。 不会返回嵌套类型。|
|GetField|1024|指定应返回指定字段的值。此标志会传递给 **InvokeMember** 方法以获取字段值。|
|GetProperty|4096|指定应返回指定属性的值。此标志会传递给 **InvokeMember** 方法以调用属性 getter。|
|IgnoreCase|1|指定在绑定时不应考虑成员名称的大小写。|
|IgnoreReturn|16777216|在 COM 互操作中用于指定可以忽略成员的返回值。|
|Instance|4|指定实例成员要包括在搜索中。|
|InvokeMethod|256|指定要调用的方法。 这不必是构造函数或类型初始值设定项。此标志会传递给 **InvokeMember** 方法以调用方法。|
|NonPublic|32|指定非公共成员要包括在搜索中。|
|OptionalParamBinding|262144|返回其参数计数与提供的参数数量匹配的成员集。 此绑定标志用于参数具有默认值的方法和使用变量参数 (varargs) 的方法。 此标志只应与 **InvokeMember**(String, BindingFlags, Binder, Object, Object[], ParameterModifier[], CultureInfo, String[]) 结合使用。使用默认值的参数仅在省略了尾随参数的调用中使用。 它们必须是位于最后面的参数。|
|Public|16|指定公共成员要包括在搜索中。|
|PutDispProperty|16384|指定应调用 COM 对象上的 PROPPUT 成员。 PROPPUT 指定使用值的属性设置函数。 如果属性同时具有 PutDispProperty 和 PROPPUT 并且你需要区分调用哪一个，请使用 PROPPUTREF。|
|PutRefDispProperty|32768|指定应调用 COM 对象上的 PROPPUTREF 成员。 PROPPUTREF 指定使用引用而不是值的属性设置函数。 如果属性同时具有 PutRefDispProperty 和 PROPPUT 并且你需要区分调用哪一个，请使用 PROPPUTREF。|
|SetField|2048|指定应设置指定字段的值。此标志会传递给 **InvokeMember** 方法以设置字段值。
|SetProperty|8192|指定应设置指定属性的值。 对于 COM 属性，指定此绑定标志等效于指定 PutDispProperty 和 PutRefDispProperty。此标志会传递给 InvokeMember 方法以调用属性 setter。|
|Static|8|指定静态成员要包括在搜索中。|
|SuppressChangeType|131072|未实现。|

## 3、代码分析
```cs
public class TestClass
{
    public String Name;
    private Object[] values = new Object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public Object this[int index]
    {
        get
        {
            return values[index];
        }
        set
        {
            values[index] = value;
        }
    }

    public Object Value
    {
        get
        {
            return "the value";
        }
    }

    public TestClass() : this("initialName") { }
    public TestClass(string initName)
    {
        Name = initName;
    }

    int methodCalled = 0;

    public static void SayHello()
    {
        Console.WriteLine("Hello");
    }

    public void AddUp()
    {
        methodCalled++;
        Console.WriteLine("AddUp Called {0} times", methodCalled);
    }

    public static double ComputeSum(double d1, double d2)
    {
        return d1 + d2;
    }

    public static void PrintName(String firstName, String lastName)
    {
        Console.WriteLine("{0},{1}", lastName, firstName);
    }

    public void PrintTime()
    {
        Console.WriteLine(DateTime.Now);
    }

    public void Swap(ref int a, ref int b)
    {
        int x = a;
        a = b;
        b = x;
    }
}

[DefaultMemberAttribute("PrintTime")]
public class TestClass2
{
    public void PrintTime()
    {
        Console.WriteLine(DateTime.Now);
    }
}

public class Base
{
    static int BaseOnlyPrivate = 0;
    protected static int BaseOnly = 0;
}
public class Derived : Base
{
    public static int DerivedOnly = 0;
}
public class MostDerived : Derived { }
```
### 3.1、**BindingFlags.Static**
```cs
//Invoking a static method
Type t = typeof(TestClass);
t.InvokeMember("SayHello", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, null, new object[] { });

// BindingFlags.InvokeMethod
// Call an instance method.
TestClass c = new TestClass();
Console.WriteLine();
Console.WriteLine("Invoking an instance method.");
Console.WriteLine("----------------------------");
c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });
c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });
```  
* **"SayHello"** 指定成员名
* **BindingFlags.InvokeMethod** 指定行为为调用方法，如果单独指定，`BindingFlags.Public`、`BindingFlags.Instance`和 `BindingFlags.Static` 将自动包括在内。
* **BindingFlags.Public** 指定访问权限
* **BindingFlags.Static** 指定成员类型

### 3.2、传入参数
```cs
object[] args = new object[] { 100.09, 184.45 };
object result;
Console.WriteLine();
Console.WriteLine("Invoking a method with parameters.");
Console.WriteLine("---------------------------------");
result = t.InvokeMember("ComputeSum", BindingFlags.InvokeMethod, null, null, args);
Console.WriteLine("{0} + {1} = {2}", args[0], args[1], result);
```
* 数组长度必须和参数个数相同

### 3.3、属性字段
```cs
Console.WriteLine("Invoking a field (getting and setting.)");
Console.WriteLine("--------------------------------------");
// Get a field value.
result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
Console.WriteLine("Name == {0}", result);
// Set a field.
t.InvokeMember("Name", BindingFlags.SetField, null, c, new object[] { "NewName" });
result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
Console.WriteLine("Name == {0}", result);

Console.WriteLine("Invoking an indexed property (getting and setting.)");
Console.WriteLine("--------------------------------------------------");
// BindingFlags.GetProperty
// Get an indexed property value.
int index = 3;
result = t.InvokeMember("Item", BindingFlags.GetProperty, null, c, new object[] { index });
Console.WriteLine("Item[{0}] == {1}", index, result);
// BindingFlags.SetProperty
// Set an indexed property value.
index = 3;
t.InvokeMember("Item", BindingFlags.SetProperty, null, c, new object[] { index, "NewValue" });
result = t.InvokeMember("Item", BindingFlags.GetProperty, null, c, new object[] { index });
Console.WriteLine("Item[{0}] == {1}", index, result);


Console.WriteLine("Getting a field or property.");
Console.WriteLine("----------------------------");
// BindingFlags.GetField
// Get a field or property.
result = t.InvokeMember("Name", BindingFlags.GetField | BindingFlags.GetProperty, null, c, new object[] { });
Console.WriteLine("Name == {0}", result);
// BindingFlags.GetProperty
result = t.InvokeMember("Value", BindingFlags.GetField | BindingFlags.GetProperty, null, c, new object[] { });
Console.WriteLine("Value == {0}", result);
```
* 处理索引属性时，参数数组的第一个内容为索引键。

### 3.4、具名参数
```cs
Console.WriteLine("Invoking a method with named parameters.");
Console.WriteLine("---------------------------------------");
// BindingFlags.InvokeMethod
// Call a method using named parameters.
object[] argValues = new object[] { "Mouse", "Micky" };
String[] argNames = new String[] { "lastName", "firstName" };
t.InvokeMember("PrintName", BindingFlags.InvokeMethod, null, null, argValues, null, null, argNames);
```
* `argNames` 与 `argValues` 对应。