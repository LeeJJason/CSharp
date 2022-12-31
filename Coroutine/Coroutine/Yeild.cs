using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coroutine
{
    class Yeild
    {
        public static IEnumerator Run()
        {
            yield return 1;
            Console.WriteLine("Surprise");
            yield return 3;
            yield break;
            yield return 4;
        }
    }
}
