# C# 中 struct 实现接口的注意事项
[原文地址](https://zhuanlan.zhihu.com/p/37072966)

```cs
interface IShape
{
    float area { get; }
    void Move(float deltaX, float deltaY);
}

struct Circle : IShape
{
    public Vector2 center;
    public float radius;

    public float area
    {
        get { return Mathf.PI * radius * radius; }
    }

    public Circle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public void Move(float deltaX, float deltaY)
    {
        center.x += deltaX;
        center.y += deltaY;
    }
}
```

测试代码：
```cs
void Test()
{
    Circle c = new Circle(Vector2.zero, 1f);
    IShape s = c;

    s.Move(2f, 2f);
    print(c.center);
}
```

结果：
```
(0.0, 0.0)
```

结论：

当使用 `interface` 类型变量引用 `struct` 对象时发生装箱，此引用指向新的引用类型对象而非原对象。因此：

1. `一般应避免`使用 `interface` 类型变量引用实现此 `interface` 的 `struct` 实例，因为可能造成性能问题。

2. `一般应避免` `struct` 实现那些提供修改对象自身的方法的 `interface`，因为可能造成语义误解。



那么 `struct` 实现 `interface` 有什么意义呢？ 看这个例子：
```cs
float TotalArea<T>(IList<T> shapes) where T : IShape
{
    float total = 0f;
    foreach (var s in shapes)
    {
        total += s.area;
    }
    return total;
}

```

`泛型类型参数约束`要求必须实现某些 `interface` 时，只要 `struct` 实现了这些 `interface`，就可以作为类型参数使用来共享实现代码了。在上面的例子里，因为是通过类型 `T` 访问 `area` 属性的而不是通过 `interface`，所以没有发生装箱不会造成性能问题。

在 C# using 语句中，要求提供一个实现 IDisposable 的对象，在作用域结束时执行其 Dispose 方法。如果该对象是 struct 类型，编译后会转化为 IDisposable 再调用吗？

```cs
struct Scope : IDisposable
{
    void IDisposable.Dispose() { }
}

static void Main(string[] args)
{
    Scope a = new Scope();
    using (a) { }
}
```

使用 IL DASM 查看编译代码如下：
```cs
.locals init ([0] valuetype ConsoleApp.Program/Scope a,
           [1] valuetype ConsoleApp.Program/Scope V_1)
IL_0000:  nop
IL_0001:  ldloca.s   a
IL_0003:  initobj    ConsoleApp.Program/Scope
IL_0009:  ldloc.0
IL_000a:  stloc.1
.try
{
  IL_000b:  nop
  IL_000c:  nop
  IL_000d:  leave.s    IL_001e
}  // end .try
finally
{
  IL_000f:  ldloca.s   V_1
  IL_0011:  constrained. ConsoleApp.Program/Scope
  IL_0017:  callvirt  instance void [mscorlib]System.IDisposable::Dispose()
  IL_001c:  nop
  IL_001d:  endfinally
}  // end handler
```

编译代码中有两点需要注意：包含两个 Scope 类型的局部变量 a 和 V_1；callvirt 发生在对象 V_1 而不是 a 上。由此可以看出，语句 using (a) 等价于 using (var V_1 = a)，这是一个容易被误解的地方。

作为对比，下面是显式装箱然后调用的代码：
```cs
.locals init ([0] valuetype ConsoleApp.Program/Scope a)
IL_0000:  nop
IL_0001:  ldloca.s   a
IL_0003:  initobj    ConsoleApp.Program/Scope
IL_0009:  ldloc.0
IL_000a:  box        ConsoleApp.Program/Scope
IL_000f:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
```

可以看到明确的执行了 box 操作。因此，using 语句不会对 struct 对象执行装箱，struct 实现 IDisposable 不会影响性能。