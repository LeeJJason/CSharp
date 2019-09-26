using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coroutine.Code
{
    /// <summary>
    /// 按帧等待
    /// </summary>
    public class WaitForFrames : IWait
    {
        private int frames = 0;
        public WaitForFrames(int frames)
        {
            this.frames = frames;
        }

        public bool Tick()
        {
            --this.frames;
            return this.frames <= 0;
        }
    }
}
