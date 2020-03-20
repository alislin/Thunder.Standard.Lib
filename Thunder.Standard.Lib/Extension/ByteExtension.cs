// <copyright file="ByteExtension.cs" author="linya">
// Create time：       2020/3/20 14:32:23
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thunder.Standard.Lib.Extension
{
    public static class ByteExtension
    {
        /// <summary>
        /// 转换为16进制字串
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] buf)
        {
            var result = "";
            foreach (var item in buf)
            {
                result += item.ToString("X2");
            }
            return result;
        }

        public static bool Find(this Stream stream,byte[] buf)
        {
            // todo: 未完成的功能
            return true;
        }
    }
}
