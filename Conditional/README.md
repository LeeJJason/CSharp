# Conditional

[Conditional（C# 编程指南）](https://docs.microsoft.com/zh-cn/previous-versions/visualstudio/visual-studio-2008/4xssyw96(v=vs.90))

## 初始代码
```cs
class Program
{
    static void Main(string[] args)
    {
        Run();
    }

    public static void Run()
    {
        Console.WriteLine("Run Test");
    }
}
```

输出结果:

`Run Test`


### 反编译代码分析
```cs
// Conditional.Program
using System;

internal class Program
{
	private static void Main(string[] args)
	{
		Run();
	}

	public static void Run()
	{
		Console.WriteLine("Run Test");
	}
}
```


## 加上条件编译
```cs
class Program
{
    static void Main(string[] args)
    {
        Run();
    }
    [Conditional("TRACE_ON")]
    public static void Run()
    {
        Console.WriteLine("Run Test");
    }
}
```

### 未定义宏 TRACE_ON
```cs
// Conditional.Program
using System;
using System.Diagnostics;

internal class Program
{
	private static void Main(string[] args)
	{
	}

	[Conditional("TRACE_ON")]
	public static void Run()
	{
		Console.WriteLine("Run Test");
	}
}
```
无输出


**原因** : 代码未调用添加 `Conditional` 属性的方法

#### 定义宏 TRACE_ON
```cs
// Conditional.Program
#define TRACE_ON
using System;
using System.Diagnostics;

internal class Program
{
	private static void Main(string[] args)
	{
		Run();
	}

	[Conditional("TRACE_ON")]
	public static void Run()
	{
		Console.WriteLine("Run Test");
	}
}
```

输出结果:

`Run Test`


## 结论
`Conditional` 属性能通过宏定义控制特性类、方法是否引用。

类似于：
```cs
internal class Program
{
	private static void Main(string[] args)
	{
#if TRACE_ON
		Run();
#endif
	}

	//[Conditional("TRACE_ON")]
	public static void Run()
	{
		Console.WriteLine("Run Test");
	}
}
```