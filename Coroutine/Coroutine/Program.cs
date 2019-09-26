using Coroutine.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coroutine
{
    public class Program
    {
        static void Main(string[] args)
        {
            var t1 = Test01();
            var t2 = Test02();
            CoroutineManager.Instance.StartCoroutine(t1);
            CoroutineManager.Instance.StartCoroutine(t2);

            while (true)
            {
                Thread.Sleep(30);
                CoroutineManager.Instance.Update();
            }
        }


        static IEnumerator Test01()
        {
            Console.WriteLine("start test 01");
            yield return new WaitForSeconds(5);
            Console.WriteLine("after 5 seconds");
            yield return new WaitForSeconds(5);
            Console.WriteLine("after 10 seconds");
        }

        static IEnumerator Test02()
        {
            Console.WriteLine("start test 02");
            yield return new WaitForFrames(500);
            Console.WriteLine("after 500 frames");
        }
    }
}
