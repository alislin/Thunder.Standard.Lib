using System;
using System.Diagnostics;

namespace Thunder.Standard.Lib.Extension
{

    public static class DateTimeExtension
    {
        /// <summary>
        /// 输出持续时长：格式（n天n小时n分n秒）
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="timeMode">输出模式（0:完整 / 1:统计到分钟 / 2:统计到小时 / 3:统计到天）</param>
        /// <param name="round">最后一位取整加权（0:无,0.5:四舍五入）</param>
        /// <returns></returns>
        public static string ToDurString(this TimeSpan ts, int timeMode = 0,double round=0)
        {
            var result = "";
            var n = ts.TotalDays;
            if (timeMode >= 3)
            {
                n += round;
                result += (int)n > 0 ? $"{(int)n}天" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{(int)n}天" : "";
            }

            n = ts.TotalHours;
            if (timeMode >= 2)
            {
                n += round;
                result += (int)n > 0 ? $"{((int)n % 24).ToString("00")}小时" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{((int)n % 24).ToString("00")}小时" : "";
            }

            n = ts.TotalMinutes;
            if (timeMode >= 1)
            {
                n += round;
                result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}分" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}分" : "";
            }
            n = ts.TotalSeconds;
            n += round;
            result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}秒" : "";
            return result;
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime time, string format)
        {
            var s = time.ToString(format);
            var d = time;
            DateTime.TryParse(s, out d);
            return d;
        }

        public static string ToViewString(this DateTime time)
        {
            var s = time.ToString("yyyy-MM-dd");
            var ts = DateTime.Now - time;
            var k = ts.TotalMinutes;
            if (k <= 5)
            {
                s = "刚刚";
            }
            else if (k <= 60)
            {
                s = $"{k.ToString("N0")}分钟前";
            }
            else if (k <= 180)
            {
                s = $"{ts.TotalHours.ToString("N0")}小时前";
            }
            else if (ts.TotalHours < 24)
            {
                s = $"{time.ToString("HH:mm")}";
            }
            else if (ts.TotalDays < 2)
            {
                s = $"昨天 {time.ToString("HH:mm")}";
            }
            else if (ts.TotalDays < 3)
            {
                s = $"前天 {time.ToString("HH:mm")}";
            }
            else if (ts.TotalDays > 365 * 5)
            {
                s = "";
            }

            return s;
        }

        public static string ToSmartShort(this DateTime time)
        {
            if (time == null) return null;

            var t = time;
            if (t.AddHours(20) < DateTime.Now)
            {
                return t.ToString("MM-dd HH:mm");
            }
            else
            {
                return t.ToString("HH:mm");
            }
        }

    }
}