using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GCNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer(OnTimer, null, 0, 3000);
            GCNorification.GCDone += OnGcDone;
            Console.ReadKey();
            timer.Dispose();
        }

        private static void OnTimer(object obj)
        {
            Console.WriteLine("OnTimer : " + DateTime.Now);
            GC.Collect();
        } 
        
        private static void OnGcDone(int generation)
        {
            Console.WriteLine("GC Done : " + generation);
        }

    }

     public static class GCNorification
    {
        private static Action<int> s_gcDone = null;
        public static event Action<int> GCDone
        {
            add
            {
                if (s_gcDone == null) { new GenObject(0); new GenObject(1); new GenObject(2); };
                s_gcDone += value;
            }
            remove
            {
                s_gcDone -= value;
            }
        }


        private sealed class GenObject
        {
            private int m_generation;
            public GenObject(int generation) { m_generation = generation; }
            ~GenObject()
            {
                if(GC.GetGeneration(this) >= m_generation)
                {
                    Action<int> temp = Volatile.Read(ref s_gcDone);
                    if (temp != null) temp(m_generation);
                }

                if((s_gcDone != null) && !AppDomain.CurrentDomain.IsFinalizingForUnload() && !Environment.HasShutdownStarted)
                {
                    if (m_generation == 0) new GenObject(0);
                    else GC.ReRegisterForFinalize(this);
                }
                else
                {

                }
            }
        }
    }
}
