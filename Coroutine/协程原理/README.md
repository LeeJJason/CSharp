[聊一聊Unity协程背后的实现原理 ](https://www.cnblogs.com/iwiniwin/p/14878498.html)

Unity开发不可避免的要用到协程(Coroutine)，协程同步代码做异步任务的特性使程序员摆脱了曾经异步操作加回调的编码方式，使代码逻辑更加连贯易读。然而在惊讶于协程的好用与神奇的同时，因为不清楚协程背后的实现原理，所以总是感觉无法完全掌握协程。比如：

1. `MonoBehaviour.StartCoroutine`接收的参数为什么是`IEnumerator`，`IEnumerator`和协程有什么关系？
2. 既然协程函数返回值声明是`IEnumerator`，为什么函数内`yield return`的又是不同类型的返回值？
3. `yield`是什么，常见的`yield return`，`yield break`是什么意思，又有什么区别？
4. 为什么使用了`yield return`就可以使代码“停”在那里，达到某种条件后又可以从“停住”的地方继续执行？
5. 具体的，`yield return new WaitForSeconds(3)`，`yield return webRequest.SendWebRequest()`，为什么可以实现等待指定时间或是等待请求完成再接着执行后面的代码？

如果你和我一样也有上面的疑问，不妨阅读下本文，相信一定可以解答你的疑惑。

### IEnumerator是什么

根据微软[官方文档](https://docs.microsoft.com/zh-cn/dotnet/api/system.collections.ienumerator?view=netframework-4.7.1)的描述，IEnumerator是所有非泛型枚举器的基接口。换而言之就是IEnumerator定义了一种适用于任意集合的迭代方式。任意一个集合只要实现自己的IEnumerator，它的使用者就可以通过IEnumerator迭代集合中的元素，而不用针对不同的集合采用不同的迭代方式。

IEnumerator的定义如下所示

```csharp
public interface IEnumerator
{
    object Current { get; }

    bool MoveNext();
    void Reset();
}
```

IEnumerator接口由一个属性和两个方法组成

1. Current属性可以获取集合中当前迭代位置的元素
2. MoveNext方法将当前迭代位置推进到下一个位置，如果成功推进到下一个位置则返回true，否则已经推进到集合的末尾返回false
3. Reset方法可以将当前迭代位置设置为初始位置（该位置位于集合中第一个元素之前，所以当调用Reset方法后，再调用MoveNext方法，Curren值则为集合的第一个元素）

比如我们经常会使用的[foreach](https://docs.microsoft.com/zh-cn/dotnet/csharp/iterators#deeper-dive-into-foreach)关键字遍历集合，其实`foreach`只是C#提供的语法糖而已

```csharp
foreach (var item in collection)
{
   Console.WriteLine(item.ToString());
}
```

本质上`foreach`循环也是采用IEnumerator来遍历集合的。在编译时编译器会将上面的`foreach`循环转换为类似于下面的代码

```csharp
{
    var enumerator = collection.GetEnumerator();
    try
    {
        while (enumerator.MoveNext())  // 判断是否成功推进到下一个元素（可理解为集合中是否还有可供迭代的元素）
        {
            var item = enumerator.Current;
            Console.WriteLine(item.ToString());
        }
    } finally
    {
        // dispose of enumerator.
    }
}
```

### yield和IEnumerator什么关系

[yield](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/yield)是C#的关键字，其实就是快速定义迭代器的语法糖。只要是`yield`出现在其中的方法就会被编译器自动编译成一个迭代器，对于这样的函数可以称之为迭代器函数。迭代器函数的返回值就是自动生成的迭代器类的一个对象

试试想象如果没有`yield`关键字，我们每定义一个迭代器，就要创建一个类，实现`IEnumerator`接口，接口包含的属性与方法都要正确的实现，是不是很麻烦？而利用`yield`关键字，只需要下面简单的几行代码，就可以快速定义一个迭代器。诸如迭代器类的创建，`IEnumerator`接口的实现工作编译器通通帮你做了

```csharp
// 由迭代器函数定义的迭代器
IEnumerator Test()
{
    yield return 1;
    Debug.Log("Surprise");
    yield return 3;
    yield break;
    yield return 4;
}
```

1. `yield return`语句可以返回一个值，表示迭代得到的当前元素
2. `yield break`语句可以用来终止迭代，表示当前没有可被迭代的元素了

如下所示，可以通过上面代码定义的迭代器遍历元素

```csharp
IEnumerator enumerator = Test();  // 直接调用迭代器函数不会执行方法的主体，而是返回迭代器对象
bool ret = enumerator.MoveNext();
Debug.Log(ret + " " + enumerator.Current);  // (1)打印：True 1
ret = enumerator.MoveNext();
// (2)打印：Surprise
Debug.Log(ret + " " + enumerator.Current);  // (3)打印：True 3
ret = enumerator.MoveNext();
Debug.Log(ret + " " + enumerator.Current);  // (4)打印：False 3
```

(1)(3)(4)处的打印都没有什么问题，(1)(3)正确打印出了返回的值，(4)是因为迭代被`yield break`终止了，所以`MoveNext`返回了false

重点关注(2)打印的位置，是在第二次调用`MoveNext`函数之后触发的，也就是说如果不调用第二次的`MoveNext`，(2)打印将不会被触发，也意味着`Debug.Log("Surprise")`这句代码不会被执行。表现上来看`yield return 1`好像把代码“停住”了，当再次调用`MoveNext`方法后，代码又从“停住”的地方继续执行了

### yield return为什么能“停住”代码

想要搞清楚代码“停住”又原位恢复的原理，就要去IL中找答案了。但是编译生成的IL是类似于汇编语言的中间语言，比较底层且晦涩难懂。所以我利用了Unity的IL2CPP，它会将C#编译生成的IL再转换成C++语言。可以通过C++代码的实现来曲线研究`yield return`的实现原理

比如下面的C#类，为了便于定位函数内的变量，所以变量名就起的复杂点

```csharp
public class Test
{
    public IEnumerator GetSingleDigitNumbers()
    {
        int m_tag_index = 0;
        int m_tag_value = 0;
        while (m_tag_index < 10)
        {
            m_tag_value += 456;
            yield return m_tag_index++;
        }
    }
}
```

生成的类在Test.cpp文件中，由于文件比较长，所以只截取部分重要的片段（有删减，完整的文件可以查看[这里](https://github.com/iwiniwin/ResourceLibrary/blob/master/unity/archive/Test.cpp)）

```none
// Test/<GetSingleDigitNumbers>d__0
struct U3CGetSingleDigitNumbersU3Ed__0_t9371C0E193B6B7701AD95F88620C6D6C93705F1A  : public RuntimeObject
{
public:
	// System.Int32 Test/<GetSingleDigitNumbers>d__0::<>1__state
	int32_t ___U3CU3E1__state_0;
	// System.Object Test/<GetSingleDigitNumbers>d__0::<>2__current
	RuntimeObject * ___U3CU3E2__current_1;
	// Test Test/<GetSingleDigitNumbers>d__0::<>4__this
	Test_tD0155F04059CC04891C1AAC25562964CCC2712E3 * ___U3CU3E4__this_2;
	// System.Int32 Test/<GetSingleDigitNumbers>d__0::<m_tag_index>5__1
	int32_t ___U3Cm_tag_indexU3E5__1_3;
	// System.Int32 Test/<GetSingleDigitNumbers>d__0::<m_tag_value>5__2
	int32_t ___U3Cm_tag_valueU3E5__2_4;

public:
	inline int32_t get_U3CU3E1__state_0() const { return ___U3CU3E1__state_0; }
	inline void set_U3CU3E1__state_0(int32_t value)
	{
		___U3CU3E1__state_0 = value;
	}

	inline RuntimeObject * get_U3CU3E2__current_1() const { return ___U3CU3E2__current_1; }
	inline void set_U3CU3E2__current_1(RuntimeObject * value)
	{
		___U3CU3E2__current_1 = value;
		Il2CppCodeGenWriteBarrier((void**)(&___U3CU3E2__current_1), (void*)value);
	}

	inline int32_t get_U3Cm_tag_indexU3E5__1_3() const { return ___U3Cm_tag_indexU3E5__1_3; }
	inline void set_U3Cm_tag_indexU3E5__1_3(int32_t value)
	{
		___U3Cm_tag_indexU3E5__1_3 = value;
	}

	inline int32_t get_U3Cm_tag_valueU3E5__2_4() const { return ___U3Cm_tag_valueU3E5__2_4; }
	inline void set_U3Cm_tag_valueU3E5__2_4(int32_t value)
	{
		___U3Cm_tag_valueU3E5__2_4 = value;
	}
};
```

可以看到`GetSingleDigitNumbers`函数确实被定义成了一个类`U3CGetSingleDigitNumbersU3Ed__0_t9371C0E193B6B7701AD95F88620C6D6C93705F1A`，而局部变量`m_tag_index`和`m_tag_value`都分别被定义成了这个类的成员变量`___U3Cm_tag_indexU3E5__1_3`和`___U3Cm_tag_valueU3E5__2_4`，并且为它们生成了对应的get和set方法。`___U3CU3E2__current_1`成员变量对应`IEnumerator`的`Current`属性。这里再关注下额外生成的`___U3CU3E1__state_0`成员变量，可以理解为一个状态机，通过它表示的不同状态值，决定了整个函数逻辑应该如何执行，后面会看到它是如何起作用的。

```cpp
// System.Boolean Test/<GetSingleDigitNumbers>d__0::MoveNext()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool U3CGetSingleDigitNumbersU3Ed__0_MoveNext_mED8994A78E174FF0A8BE28DF873D247A3F648CFB (U3CGetSingleDigitNumbersU3Ed__0_t9371C0E193B6B7701AD95F88620C6D6C93705F1A * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (U3CGetSingleDigitNumbersU3Ed__0_MoveNext_mED8994A78E174FF0A8BE28DF873D247A3F648CFB_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	bool V_2 = false;
	{
		int32_t L_0 = __this->get_U3CU3E1__state_0();
		V_0 = L_0;
		int32_t L_1 = V_0;
		if (!L_1)
		{
			goto IL_0012;
		}
	}
	{
		goto IL_000c;
	}

IL_000c:
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)1)))
		{
			goto IL_0014;
		}
	}
	{
		goto IL_0016;
	}

IL_0012:
	{
		goto IL_0018;
	}

IL_0014:
	{
		goto IL_0068;
	}

IL_0016:
	{
		return (bool)0;
	}

IL_0018:
	{
		__this->set_U3CU3E1__state_0((-1));
		// int m_tag_index = 0;
		__this->set_U3Cm_tag_indexU3E5__1_3(0);
		// int m_tag_value = 0;
		__this->set_U3Cm_tag_valueU3E5__2_4(0);
		goto IL_0070;
	}

IL_0030:
	{
		// m_tag_value += 456;
		int32_t L_3 = __this->get_U3Cm_tag_valueU3E5__2_4();
		__this->set_U3Cm_tag_valueU3E5__2_4(((int32_t)il2cpp_codegen_add((int32_t)L_3, (int32_t)((int32_t)456))));
		// yield return m_tag_index++;
		int32_t L_4 = __this->get_U3Cm_tag_indexU3E5__1_3();
		V_1 = L_4;
		int32_t L_5 = V_1;
		__this->set_U3Cm_tag_indexU3E5__1_3(((int32_t)il2cpp_codegen_add((int32_t)L_5, (int32_t)1)));
		int32_t L_6 = V_1;
		int32_t L_7 = L_6;
		RuntimeObject * L_8 = Box(Int32_t585191389E07734F19F3156FF88FB3EF4800D102_il2cpp_TypeInfo_var, &L_7);
		__this->set_U3CU3E2__current_1(L_8);
		__this->set_U3CU3E1__state_0(1);
		return (bool)1;
	}

IL_0068:
	{
		__this->set_U3CU3E1__state_0((-1));
	}

IL_0070:
	{
		// while (m_tag_index < 10)
		int32_t L_9 = __this->get_U3Cm_tag_indexU3E5__1_3();
		V_2 = (bool)((((int32_t)L_9) < ((int32_t)((int32_t)10)))? 1 : 0);
		bool L_10 = V_2;
		if (L_10)
		{
			goto IL_0030;
		}
	}
	{
		// }
		return (bool)0;
	}
}
```

而`U3CGetSingleDigitNumbersU3Ed__0_MoveNext_mED8994A78E174FF0A8BE28DF873D247A3F648CFB `成员方法对应了`IEnumerator`的`MoveText`方法。它的实现利用了goto语句，而这个方法正是代码“停住”与恢复的关键所在

我们一步步来看，按照c#代码的逻辑，第一次调用`moveNext`函数时，应该执行以下代码

```csharp
int m_tag_index = 0;
int m_tag_value = 0;
if (m_tag_index < 10)
{
    m_tag_value += 456;
    return m_tag_index++;
}
```

对应执行的c++代码如下所示。执行完毕IL_0030完毕后，将返回true，表示还有元素。此时的state为1

```cpp
// 初始时，___U3CU3E1__state_0值为0
goto IL_0012;
goto IL_0018;  // IL_0018内部初始化m_tag_index和m_tag_value为0. 同时设置___U3CU3E1__state_0值为-1
goto IL_0070;  // 判断m_tag_index是否小于10
goto IL_0030;  // IL_0030内部将m_tag_index值加1，并将m_tag_index的值设置为current值，并将___U3CU3E1__state_0值设置为1
```

第二次调用`moveNext`函数，对应C#代码为

```csharp
if (m_tag_index < 10)
{
    m_tag_value += 456;
    return m_tag_index++;
}
```

对应的c++代码为

```cpp
// 此时___U3CU3E1__state_0值为1，根据判断进入IL_000c
goto IL_000c;
goto IL_0014;
goto IL_0068;  // 设置___U3CU3E1__state_0为-1
IL_0070  // 判断m_tag_index是否小于10
goto IL_0030;  // 返回1，表示true，还有可迭代元素
```

当第11次调用`moveNext`函数时，`m_tag_index`的值已经是10，此时函数应该结束。返回值应该是false，表示没有再能返回的元素了。
所以对应的C++代码为

```cpp
// ___U3CU3E1__state_0值是1
goto IL_000c;
goto IL_0014;
goto IL_0068
IL_0070  // 判断m_tag_index是不小于10的，所以不会进入IL_0030
{
	// }
	return (bool)0;  
}
```

到这里，我想代码“停住”与恢复的神秘面纱终于被揭开了。总结下来就是，以能“停住”的地方为分界线，编译器会为不同分区的语句按照功能逻辑生成一个个对应的代码块。`yield`语句就是这条分界线，想要代码“停住”，就不执行后面语句对应的代码块，想要代码恢复，就接着执行后面语句对应的代码块。而调度上下文的保存，是通过将需要保存的变量都定义成成员变量来实现的。

### Unity协程机制的实现原理

现在我们可以讨论下`yield return`与协程的关系了，或者说IEnumerator与协程的关系

协程是一种比线程更轻量级的存在，协程可完全由用户程序控制调度。协程可以通过yield方式进行调度转移执行权，调度时要能够保存上下文，在调度回来的时候要能够恢复。这是不是和上面“停住”代码然后又原位恢复的执行效果很像？没错，Unity实现协程的原理，就是通过`yield return`生成的`IEnumerator`再配合控制何时触发`MoveNext`来实现了执行权的调度

具体而言，Unity每通过`MonoBehaviour.StartCoroutine`启动一个协程，就会获得一个`IEnumerator`（`StartCoroutine`的参数就是`IEnumerator`，参数是方法名的重载版本也会通过反射拿到该方法对应的`IEnumerator`）。并在它的游戏循环中，根据条件判断是否要执行`MoveNext`方法。而这个条件就是根据`IEnumerator`的`Current`属性获得的，即`yield return`返回的值。

在启动一个协程时，Unity会先调用得到的`IEnumerator`的`MoveNext`一次，以拿到`IEnumerator`的`Current`值。所以每启动一个协程，协程函数会立即执行到第一个`yield return`处然后“停住”。

对于不同的`Current`类型（一般是`YieldInstruction`的子类），Unity已做好了一些默认处理，比如：

- 如果`Current`是`null`，就相当于什么也不做。在下一次游戏循环中，就会调用`MoveNext`。所以`yield return null`就起到了等待一帧的作用

- 如果`Current`是`WaitForSeconds`类型，Unity会获取它的等待时间，每次游戏循环中都会判断时间是否到了，只有时间到了才会调用`MoveNext`。所以`yield return WaitForSeconds`就起到了等待指定时间的作用

- 如果`Current`是`UnityWebRequestAsyncOperation`类型，它是`AsyncOperation`的子类，而`AsyncOperation`有`isDone`属性，表示操作是否完成，只有`isDone`为true时，Unity才会调用`MoveNext`。对于`UnityWebRequestAsyncOperation`而言，只有请求完成了，才会将`isDone`属性设置为true。

    也因此我们才可以使用下面的同步代码，完成本来是异步的网络请求操作。

    ```csharp
    using(UnityWebRequest webRequest = UnityWebRequest.Get("https://www.cnblogs.com/iwiniwin/p/13705456.html"))
    {
        yield return webRequest.SendWebRequest();
        if(webRequest.isNetworkError)
        {
            Debug.Log("Error " + webRequest.error);
        }
        else
        {
            Debug.Log("Received " + webRequest.downloadHandler.text);
        }
    }
    ```

### 实现自己的Coroutine

Unity的协程是和MonoBehavior进行了绑定的，只能通过`MonoBehavior.StartCoroutine`开启协程，而在开发中，有些不是继承MonoBehavior的类就无法使用协程了，在这种情况下我们可以自己封装一套协程。在搞清楚Unity协程的实现原理后，想必实现自己的协程也不是难事了，感兴趣的同学赶快行动起来吧。

[这里](https://github.com/iwiniwin/unity-remote-file-explorer/blob/main/Runtime/Common/Coroutines.cs)有一份[Remote File Explorer](https://github.com/iwiniwin/unity-remote-file-explorer)内已经封装好的实现，被用于制作Editor工具时无法使用MonoBehavior又想使用协程的情况下。[Remote File Explorer](https://github.com/iwiniwin/unity-remote-file-explorer)是一个跨平台的远程文件浏览器，使用户通过Unity Editor就能操作应用所运行平台上的目录文件，其内部消息通讯部分大量使用了协程，是了解协程同步代码实现异步任务特性的不错的例子

当然Unity Editor下使用协程，Unity也提供了相关的包，可以参考[Editor Coroutines](https://docs.unity3d.com/Packages/com.unity.editorcoroutines@1.0/manual/index.html)