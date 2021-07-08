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
            Run();
        }
        [Conditional("TRACE_ON")]
        public static void Run()
        {
            Console.WriteLine("Run Test");
        }
    }
}
