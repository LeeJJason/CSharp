using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BindingFlagsPart
{
    class Program
    {
        static void Main()
        {
            //InvokeMemberPart();

            BindingFlagsPart();
            
        }

        public static void BindingFlagsPart()
        {
            // BindingFlags.InvokeMethod
            // Call a static method.
            Type t = typeof(TestClass);

            Console.WriteLine();
            Console.WriteLine("Invoking a static method.");
            Console.WriteLine("-------------------------");
            t.InvokeMember("SayHello", BindingFlags.InvokeMethod | BindingFlags.Public |
                BindingFlags.Static, null, null, new object[] { });

            // BindingFlags.InvokeMethod
            // Call an instance method.
            TestClass c = new TestClass();
            Console.WriteLine();
            Console.WriteLine("Invoking an instance method.");
            Console.WriteLine("----------------------------");
            c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });
            c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });

            // BindingFlags.InvokeMethod
            // Call a method with parameters.
            object[] args = new object[] { 100.09, 184.45 };
            object result;
            Console.WriteLine();
            Console.WriteLine("Invoking a method with parameters.");
            Console.WriteLine("---------------------------------");
            result = t.InvokeMember("ComputeSum", BindingFlags.InvokeMethod, null, null, args);
            Console.WriteLine("{0} + {1} = {2}", args[0], args[1], result);

            // BindingFlags.GetField, SetField
            Console.WriteLine();
            Console.WriteLine("Invoking a field (getting and setting.)");
            Console.WriteLine("--------------------------------------");
            // Get a field value.
            result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
            Console.WriteLine("Name == {0}", result);
            // Set a field.
            t.InvokeMember("Name", BindingFlags.SetField, null, c, new object[] { "NewName" });
            result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
            Console.WriteLine("Name == {0}", result);

            Console.WriteLine();
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

            Console.WriteLine();
            Console.WriteLine("Getting a field or property.");
            Console.WriteLine("----------------------------");
            // BindingFlags.GetField
            // Get a field or property.
            result = t.InvokeMember("Name", BindingFlags.GetField | BindingFlags.GetProperty, null, c,
                new object[] { });
            Console.WriteLine("Name == {0}", result);
            // BindingFlags.GetProperty
            result = t.InvokeMember("Value", BindingFlags.GetField | BindingFlags.GetProperty, null, c,
                new object[] { });
            Console.WriteLine("Value == {0}", result);

            Console.WriteLine();
            Console.WriteLine("Invoking a method with named parameters.");
            Console.WriteLine("---------------------------------------");
            // BindingFlags.InvokeMethod
            // Call a method using named parameters.
            object[] argValues = new object[] { "Mouse", "Micky" };
            String[] argNames = new String[] { "lastName", "firstName" };
            t.InvokeMember("PrintName", BindingFlags.InvokeMethod, null, null, argValues, null, null,
                            argNames);

            Console.WriteLine();
            Console.WriteLine("Invoking a default member of a type.");
            Console.WriteLine("------------------------------------");
            // BindingFlags.Default
            // Call the default member of a type.
            Type t3 = typeof(TestClass2);
            t3.InvokeMember("", BindingFlags.InvokeMethod | BindingFlags.Default, null, new TestClass2(),
                            new object[] { });

            // BindingFlags.Static, NonPublic, and Public
            // Invoking a member with ref parameters.
            Console.WriteLine();
            Console.WriteLine("Invoking a method with ref parameters.");
            Console.WriteLine("--------------------------------------");
            MethodInfo m = t.GetMethod("Swap");
            args = new object[2];
            args[0] = 1;
            args[1] = 2;
            m.Invoke(new TestClass(), args);
            Console.WriteLine("{0}, {1}", args[0], args[1]);

            // BindingFlags.CreateInstance
            // Creating an instance with a parameterless constructor.
            Console.WriteLine();
            Console.WriteLine("Creating an instance with a parameterless constructor.");
            Console.WriteLine("------------------------------------------------------");
            object cobj = t.InvokeMember("TestClass", BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.CreateInstance,
                null, null, new object[] { });
            Console.WriteLine("Instance of {0}<{1}> created.", cobj.GetType().Name,((TestClass)cobj).Name);

            // Creating an instance with a constructor that has parameters.
            Console.WriteLine();
            Console.WriteLine("Creating an instance with a constructor that has parameters.");
            Console.WriteLine("------------------------------------------------------------");
            cobj = t.InvokeMember("TestClass", BindingFlags.Public |
                BindingFlags.Instance | BindingFlags.CreateInstance,
                null, null, new object[] { "Hello, World!" });
            Console.WriteLine("Instance of {0} created with initial value '{1}'.", cobj.GetType().Name,
                cobj.GetType().InvokeMember("Name", BindingFlags.GetField, null, cobj, null));

            // BindingFlags.DeclaredOnly
            Console.WriteLine();
            Console.WriteLine("DeclaredOnly instance members.");
            Console.WriteLine("------------------------------");
            System.Reflection.MemberInfo[] memInfo =
                t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                BindingFlags.Public);
            for (int i = 0; i < memInfo.Length; i++)
            {
                Console.WriteLine(memInfo[i].Name);
            }

            // BindingFlags.IgnoreCase
            Console.WriteLine();
            Console.WriteLine("Using IgnoreCase and invoking the PrintName method.");
            Console.WriteLine("---------------------------------------------------");
            t.InvokeMember("printname", BindingFlags.IgnoreCase | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[]
                {"Brad","Smith"});

            // BindingFlags.FlattenHierarchy
            Console.WriteLine();
            Console.WriteLine("Using FlattenHierarchy to get inherited static protected and public members.");
            Console.WriteLine("----------------------------------------------------------------------------");
            FieldInfo[] finfos = typeof(MostDerived).GetFields(BindingFlags.NonPublic | BindingFlags.Public |
                  BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo finfo in finfos)
            {
                Console.WriteLine("{0} defined in {1}.", finfo.Name, finfo.DeclaringType.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Without FlattenHierarchy.");
            Console.WriteLine("-------------------------");
            finfos = typeof(MostDerived).GetFields(BindingFlags.NonPublic | BindingFlags.Public |
                  BindingFlags.Static);
            foreach (FieldInfo finfo in finfos)
            {
                Console.WriteLine("{0} defined in {1}.", finfo.Name, finfo.DeclaringType.Name);
            }
        }

        public static void InvokeMemberPart()
        {
            Type t = typeof(MyType);
            // Create an instance of a type.
            Object[] args = new Object[] { 8 };
            Console.WriteLine("The value of x before the constructor is called is {0}.", args[0]);
            Object obj = t.InvokeMember(null,
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.CreateInstance, null, null, args);
            Console.WriteLine("Type: " + obj.GetType().ToString());
            Console.WriteLine("The value of x after the constructor returns is {0}.", args[0]);

            // Read and write to a field.
            t.InvokeMember("myField",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetField, null, obj, new Object[] { 5 });
            Int32 v = (Int32)t.InvokeMember("myField",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetField, null, obj, null);
            Console.WriteLine("myField: " + v);

            // Call a method.
            String s = (String)t.InvokeMember("ToString",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, null);
            Console.WriteLine("ToString: " + s);

            // Read and write a property. First, attempt to assign an
            // invalid value; then assign a valid value; finally, get
            // the value.
            try
            {
                // Assign the value zero to MyProp. The Property Set 
                // throws an exception, because zero is an invalid value.
                // InvokeMember catches the exception, and throws 
                // TargetInvocationException. To discover the real cause
                // you must catch TargetInvocationException and examine
                // the inner exception. 
                t.InvokeMember("MyProp",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { 0 });
            }
            catch (TargetInvocationException e)
            {
                // If the property assignment failed for some unexpected
                // reason, rethrow the TargetInvocationException.
                if (e.InnerException.GetType() !=
                    typeof(ArgumentOutOfRangeException))
                    throw;
                Console.WriteLine("An invalid value was assigned to MyProp. : " + e.InnerException.Message);
            }
            t.InvokeMember("MyProp",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { 2 });
            v = (Int32)t.InvokeMember("MyProp",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetProperty, null, obj, null);
            Console.WriteLine("MyProp: " + v);
        }
    }

    class MyType
    {
        Int32 myField;
        public MyType(ref Int32 x) { x *= 5; }
        public override String ToString() { return myField.ToString(); }
        public Int32 MyProp
        {
            get { return myField; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "value must be > 0");
                myField = value;
            }
        }
    }

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
}
