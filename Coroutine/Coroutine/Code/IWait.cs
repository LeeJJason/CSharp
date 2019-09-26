using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coroutine.Code
{
    /// <summary>
    /// 等待接口
    /// </summary>
    interface IWait
    {
        /// <summary>
        /// 每帧检测是否等待结束
        /// </summary>
        /// <returns>是否完成</returns>
        bool Tick();
    }
}
