using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Thunder.Standard.Lib.Extension
{
    public static class StringExt
    {
        /// <summary>
        /// 获取字串中的数字
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static List<int> ToIntList(this string dat)
        {
            var slist = new List<int>();
            Regex regex = new Regex(@"\d+");
            var m = regex.Matches(dat);
            foreach (Match item in m)
            {
                if(Int32.TryParse(item.Value, out var s))
                    slist.Add(s);
                //var s = Convert.ToInt32(item.Value);
            }
            return slist;
        }

        /// <summary>
        /// 转换null字串为""
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NotNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "" : s;
        }

        /// <summary>
        /// 字符串为空或者是空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 剪贴板表格内容转换为List
        /// </summary>
        /// <param name="data">剪贴板数据</param>
        /// <param name="expandAll">包含行内制表符展开</param>
        /// <param name="removeStr">移除字串列表</param>
        /// <returns></returns>
        public static List<string> ClipToList(this string data, bool expandAll = true, List<string> removeStr = null)
        {
            var list = new List<string>();
            var s = data.Replace("\r\n", "\r");
            if (removeStr == null)
            {
                removeStr = new List<string>(new string[] { "\"", "'" });
            }
            foreach (var item in removeStr)
            {
                s = s.Replace(item, "");
            }
            var m = s.Split('\r');
            foreach (var item in m)
            {
                var mm = item.Trim();
                if (string.IsNullOrWhiteSpace(mm))
                    continue;
                if (expandAll)
                {
                    var nn = mm.Split('\t').ToList();
                    foreach (var x in nn)
                    {
                        var v = x.Trim();
                        if (!v.IsNull())
                        {
                            var vl = v.ToList(" ,、/\\");
                            list.AddRange(vl);
                        }
                    }
                }
                else
                {
                    list.Add(mm);
                }
            }
            return list;
        }

        public  static List<string> ToList(this string data,string splitString)
        {
            var list = new List<string>();
            var s = new StringBuilder(splitString);
            var s1 = s[0];
            var l = data.Split(s1);
            foreach (var item in l)
            {
                if (s.Length>1)
                {
                    var listsub = item.ToList(s.ToString(1, s.Length - 1));
                    list.AddRange(listsub);
                }
                else
                {
                    var v = item.Trim();
                    if (!v.IsNull())
                        list.Add(v);
                }
            }
            return list;
        }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">关键词</param>
        /// <param name="layout">通配模式</param>
        /// <returns></returns>
        /// <remarks>
        /// 0   使用 "_" 作为通配符
        /// >1   自动使用N个通配符
        /// </remarks>
        public static bool Like(this string data, string key, int layout = 1, bool partCompare = false, int keyMaxLength = 6)
        {
            if (data.IsNull() || key.IsNull())
            {
                return false;
            }

            if (!partCompare && data.Length != key.Length)
            {
                return false;
            }

            if (key.Length - 2 < 0)
            {
                return false;
            }

            layout = layout >= 0 ? layout : 0;
            layout = layout <= key.Length - 1 ? layout : key.Length - 1;

            keyMaxLength = keyMaxLength <= 24 ? keyMaxLength : 24;
            keyMaxLength = keyMaxLength >= 5 ? keyMaxLength : 5;

            if (key.Length > keyMaxLength)
            {
                if (partCompare)
                {
                    return data.ToUpper().Contains(key.ToUpper());
                }
                else
                {
                    return string.Compare(data, key, true) == 0;
                }
            }

            if (layout == 0)
            {
                return data.KeyLike(key, partCompare);
            }
            var s = new StringBuilder(key);
            var keyIndex = key.LastIndexOf('_');
            keyIndex = keyIndex >= 0 ? keyIndex : 0;
            for (int i = keyIndex; i < s.Length; i++)
            {
                var keep = s[i];
                s[i] = '_';
                if (data.Like(s.ToString(), layout - 1, partCompare, keyMaxLength))
                    return true;
                s[i] = keep;
            }
            return false;
        }

        /// <summary>
        /// 通配符匹配
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">使用"_"作为通配符匹配</param>
        /// <returns></returns>
        public static bool KeyLike(this string data, string key, bool partCompare = false)
        {
            if (data.IsNull() || key.IsNull())
            {
                return false;
            }
            if (!partCompare && data.Length != key.Length)
            {
                return false;
            }
            key = key.Replace("_", "([^x00-xff]|[\\w])");
            var re = new Regex(key);
            return re.IsMatch(data);
        }
    }
}
