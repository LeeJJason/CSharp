using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coroutine
{
    class ForEach
    {
        public static void Run() 
        {
            List<ForEach> datas = new List<ForEach>();

            foreach(var data in datas) 
            {
                Console.WriteLine(data);
            }
        }
    }
}
