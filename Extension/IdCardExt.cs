using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Thunder.Standard.Lib.Extension
{
    public static class IdCardExt
    {
        private static readonly Regex CellPhoneExpression = new Regex(@"^1\d{10}$|^[\+0]*86[- ]*1\d{10}$",
            RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 是否身份证
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string dat,out (int area,int year,int month,int day,int sex) info)
        {
            info = (0, 0, 0, 0, 0);
            if (dat.IsNull() || dat.Length!=18)
            {
                return false;
            }
            
            info.area = 0;
            if (!int.TryParse(dat.Substring(0, 6), out var d)) return false;
            if (!int.TryParse(dat.Substring(6, 4), out var year)) return false;
            if (!int.TryParse(dat.Substring(10, 2), out var month)) return false;
            if (!int.TryParse(dat.Substring(12, 2), out var day)) return false;
            if (!int.TryParse(dat.Substring(14, 3), out var sex)) return false;
            info = (d, year, month, day, sex);
            return true;
        }

        public static bool IsMobile(this string target)
        {
            return !string.IsNullOrEmpty(target) && CellPhoneExpression.IsMatch(target);
        }

    }
}
