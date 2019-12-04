using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ZeroLib.DebugLog
{
    public class DebugTime
    {
        private static DebugTime m_debugtime = new DebugTime();
        public DateTime StartTime = DateTime.Now;

        /// <summary>
        /// 持续时间（毫秒）
        /// </summary>
        public double Durration=> (DateTime.Now - StartTime).TotalMilliseconds;

        public DebugTime()
        {
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// 计时重置
        /// </summary>
        public void Reset()
        {
            StartTime = DateTime.Now;
            Debug.WriteLine(string.Format("开始计时：{0}", StartTime));
        }

        public string Check()
        {
            if (StartTime > DateTime.MinValue)
            {
                double total = (DateTime.Now - StartTime).TotalMilliseconds;
                return $"耗时[{total.ToString("F2")}]毫秒。";
            }
            else
            {
                Reset();
                return $"开始计时：{StartTime}";
            }
        }

        public static DebugTime Current => m_debugtime;

    }
}
