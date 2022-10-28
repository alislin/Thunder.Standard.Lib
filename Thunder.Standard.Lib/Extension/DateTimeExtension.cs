using System;
using System.Diagnostics;

namespace Thunder.Standard.Lib.Extension
{

    public static class DateTimeExtension
    {
        /// <summary>
        /// �������ʱ������ʽ��n��nСʱn��n�룩
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="timeMode">���ģʽ��0:���� / 1:ͳ�Ƶ����� / 2:ͳ�Ƶ�Сʱ / 3:ͳ�Ƶ��죩</param>
        /// <param name="round">���һλȡ����Ȩ��0:��,0.5:�������룩</param>
        /// <returns></returns>
        public static string ToDurString(this TimeSpan ts, int timeMode = 0,double round=0)
        {
            var result = "";
            var n = ts.TotalDays;
            if (timeMode >= 3)
            {
                n += round;
                result += (int)n > 0 ? $"{(int)n}��" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{(int)n}��" : "";
            }

            n = ts.TotalHours;
            if (timeMode >= 2)
            {
                n += round;
                result += (int)n > 0 ? $"{((int)n % 24).ToString("00")}Сʱ" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{((int)n % 24).ToString("00")}Сʱ" : "";
            }

            n = ts.TotalMinutes;
            if (timeMode >= 1)
            {
                n += round;
                result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}��" : "";
                return result;
            }
            else
            {
                result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}��" : "";
            }
            n = ts.TotalSeconds;
            n += round;
            result += (int)n > 0 ? $"{((int)n % 60).ToString("00")}��" : "";
            return result;
        }

        /// <summary>
        /// ʱ��ת��
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
                s = "�ո�";
            }
            else if (k <= 60)
            {
                s = $"{k.ToString("N0")}����ǰ";
            }
            else if (k <= 180)
            {
                s = $"{ts.TotalHours.ToString("N0")}Сʱǰ";
            }
            else if (ts.TotalHours < 24)
            {
                s = $"{time.ToString("HH:mm")}";
            }
            else if (ts.TotalDays < 2)
            {
                s = $"���� {time.ToString("HH:mm")}";
            }
            else if (ts.TotalDays < 3)
            {
                s = $"ǰ�� {time.ToString("HH:mm")}";
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

        public static string ToDate(this DateTime date, bool autoSwitch = true) => autoSwitch && date.Year != DateTime.Now.Year ? date.ToLongDate() : date.ToString("M��d��");
        public static string ToLongDate(this DateTime date) => date.ToString("yyyy��M��d��");

        public static DateTime MondayOfWeek(this DateTime date)
        {
            var w = 1 - (int)date.DayOfWeek;
            w = w > 0 ? -6 : w;
            return date.AddDays(w);
        }

        /// <summary>
        /// ��ȡ�·�
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startOfMonth">ÿ�¿�ʼ����(����10ʱ�����¿�ʼ��)</param>
        /// <returns></returns>
        public static int ToMonth(this DateTime date, int startOfMonth = 1)
        {
            var m = date.Month;
            if (startOfMonth > 10 && date.Day >= startOfMonth)
            {
                m = date.AddMonths(1).Month;
            }
            return m;
        }

        public static int ToYearMonth(this DateTime date, int startOfMonth = 1)
        {
            var m = date.Year * 100 + date.Month;
            if (startOfMonth > 10 && date.Day >= startOfMonth)
            {
                var d = date.AddMonths(1);
                m = d.Year * 100 + d.Month;
            }
            return m;
        }

        public static DateTime FromYearMonth(this int month, int day = 1)
        {
            var y = month / 100;
            var m = month % 100;
            return new DateTime(y, m, day);
        }


    }
}