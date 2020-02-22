using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPart
{
    class ExecutionContextPart
    {
        public static void MainEntry()
        {
            CallContext.LogicalSetData("Name", "Jeffrey");

            ThreadPool.QueueUserWorkItem((object state) => { Console.WriteLine("1 Name = {0}", CallContext.LogicalGetData("Name")); });

            ExecutionContext.SuppressFlow();
            ThreadPool.QueueUserWorkItem((object state) => { Console.WriteLine("2 Name = {0}", CallContext.LogicalGetData("Name")); });
            ExecutionContext.RestoreFlow();
            Console.ReadLine();
        }
    }
}
