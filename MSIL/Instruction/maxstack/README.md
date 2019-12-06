* 代码分析
``` CSHARP
private void maxstack()
{
    int x, y = 1, a = 3, b = 4;
    x = y + (a - b);
}
```

``` CSHARP
.method private hidebysig instance void  maxstack() cil managed
{
  // Code size       14 (0xe)
  .maxstack  3
  .locals init ([0] int32 x,
           [1] int32 y,
           [2] int32 a,
           [3] int32 b)
  IL_0000:  nop
  IL_0001:  ldc.i4.1
  IL_0002:  stloc.1
  IL_0003:  ldc.i4.3
  IL_0004:  stloc.2
  IL_0005:  ldc.i4.4
  IL_0006:  stloc.3
  //最大 stack 分配 begin
  IL_0007:  ldloc.1
  IL_0008:  ldloc.2
  IL_0009:  ldloc.3
  //最大 stack 分配 end
  IL_000a:  sub
  IL_000b:  add
  IL_000c:  stloc.0
  IL_000d:  ret
} // end of method Program::maxstack
```

.maxstack is part of the IL verification. Basically .maxstack tells the JIT the max stack size it needs to reserve for the method. For example, x = y + (a - b) translates to

(Pseudo IL:)
```
1. Push y on the stack
2. Push a on the stack
3. Push b on the stack
4. Pop the last two items from the stack,
    substract them and
    push the result on the stack
5. Pop the last two items from the stack,
    add them and
    push the result on the stack
6. Store the last item on the stack in x and
    pop the last item from the stack
```
As you can see, there are at most 3 items on the stack at each time. If you'd set .maxstack to 2 (or less) for this method, the code wouldn't run.

Also, you cannot have something like this as it would require an infinite stack size:
```
1. Push x on the stack
2. Jump to step 1
```
To answer your questions:  
1. does this apply just for the function, or to all the functions that we are   calling for?  
**Just for the function**
2. even if it's just for the function were .maxstack is being declared, how do you know what maxstack is if you have branching? You go and see all the "paths" and return the maximum value possible?
**You go and see all the paths and return the maximum value possible**
3. What happens if I set it to 16 and actually there are 17 variables?
**It's unrelated to the number of variables, see Lasse V. Karlsen's answer**
4. Is there a too big of a penalty if I set it to 256?
**Doesn't seem like a good idea, but I don't know.**

*Do you really have to calculate the .maxstack yourself? System.Reflection.Emit calculates it for you IIRC.*



## 参考文章
[How does the .NET IL .maxstack directive work?
](https://stackoverflow.com/questions/1241308/how-does-the-net-il-maxstack-directive-work)