using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPart
{
    static class ThreadPoolPart
    {
        public static void MainEntry()
        {
            Console.WriteLine("Main Thread : queuing an asynchronous operation");
            ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
            Console.WriteLine("Main Thread : Doing other work here...");
            Thread.Sleep(10000);
            Console.WriteLine("Hit <Enter> to end this program");
            Console.ReadLine();
        }

        private static void ComputeBoundOp(object state)
        {
            Console.WriteLine("In ComputeBoundOp: State={0}",state);
            Thread.Sleep(1000);
        }
    }
}
