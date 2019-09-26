using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coroutine.Code
{
    /// <summary>
    /// 按秒等待
    /// </summary>
    public class WaitForSeconds : IWait
    {
        int milliseconds = 0;
        DateTime begin;

        public WaitForSeconds(float seconds)
        {
            this.begin = DateTime.Now;
            this.milliseconds = (int)(seconds * 1000);
        }

        public bool Tick()
        {
            TimeSpan span = DateTime.Now - begin;
            return span.TotalMilliseconds >= milliseconds;
        }
    }
}
