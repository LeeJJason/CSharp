using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashContainer
{
    class Program
    {
        static Dictionary<InfoClass, int> dictc = new Dictionary<InfoClass, int>();
        static Dictionary<InfoStruct, int> dicts = new Dictionary<InfoStruct, int>();

        static void Main(string[] args)
        {
            FunClass();
            System.Console.WriteLine("-----------------------------------");
            FunStruct();
            System.Console.ReadKey();
        }

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
            infoStruct1.Equals(infoStruct1);
            dicts.Add(infoStruct, 2);
            // 1、 Add操作
            dicts.Add(infoStruct1, 3);
            // 2、 [] 操作
            dicts[infoStruct2] = 3;
            System.Console.WriteLine("infoStruct => " + dicts[infoStruct]);
            System.Console.WriteLine("infoStruct1 => " + dicts[infoStruct1]);
            System.Console.WriteLine("infoStruct1 == infoStruct2 => " + (infoStruct2 == infoStruct1));

            GenericStruct(infoStruct);
        }


        public static void GenericStruct<T>(T info)
        {
            if(info == null)
            {
                Console.WriteLine("Box Test");
            }
        }

    }
}
