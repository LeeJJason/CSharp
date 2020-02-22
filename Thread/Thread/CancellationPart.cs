using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPart
{
    class CancellationPart
    {
        public static void MainEntry()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));

            Console.WriteLine("Press <Enter> to cancel the operation");
            Console.ReadLine();
            cts.Cancel();
            Console.ReadLine();
        }

        public static void MainEntry1()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() => { Console.WriteLine("Canceled 1"); });
            cts.Token.Register(() => { Console.WriteLine("Canceled 2"); });
            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));

            Console.WriteLine("Press <Enter> to cancel the operation");
            Console.ReadLine();
            cts.Cancel();
            Console.ReadLine();
        }

        public static void MainEntry2()
        {
            CancellationTokenSource cts1 = new CancellationTokenSource();
            cts1.Token.Register(() => { Console.WriteLine("cts1 Canceled"); });

            CancellationTokenSource cts2 = new CancellationTokenSource();
            cts2.Token.Register(() => { Console.WriteLine("cts2 Canceled"); });


            CancellationTokenSource linkCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);
            linkCts.Token.Register(()=> { Console.WriteLine("linkedCts canceled"); });

            ThreadPool.QueueUserWorkItem(o => Count(cts1.Token, 1000));

            Console.WriteLine("Press <Enter> to cancel the operation");
            Console.ReadLine();
            cts1.Cancel();


            Console.WriteLine("cts1 cancleed={0}, cts2 canceled={1}, linkedCts={2}", cts1.IsCancellationRequested, cts2.IsCancellationRequested, linkCts.IsCancellationRequested);
            Console.ReadLine();
        }

        private static void Count(CancellationToken token, int countTo)
        {
            for (int count = 0; count < countTo; ++count)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Count is cancelled");
                    break;
                }
                Console.WriteLine(count);
                Thread.Sleep(200);
            }
            Console.WriteLine("Count Is done");
        }

        
    }


}
