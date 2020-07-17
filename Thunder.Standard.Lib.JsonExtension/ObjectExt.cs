// <copyright file="ObjectExt.cs" author="linya">
// Create time：       2020/7/15 15:48:05
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.Standard.Lib.Extension
{
    public static class ObjectExt
    {
        /// <summary>
        /// 判断两个对象值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ValueEqual<T>(this T obja, T objb)
        {
            if (obja == null && objb == null)
            {
                return true;
            }
            if (obja == null || objb == null)
            {
                return false;
            }

            //JSON对比
            if (obja.ToJson() == objb.ToJson())
            {
                return true;
            }

            return false;
        }

    }
}
