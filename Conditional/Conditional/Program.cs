#define TRACE_ON

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditional
{
    class Program
    {
        static void Main(string[] args)
        {
            Invoker.Run();
        }
        
    }
    [Conditional("TRACE_ON")]
    class Invoker
    {
       
        public static void Run()
        {
            Console.WriteLine("Run Test");
        }
    }
}
