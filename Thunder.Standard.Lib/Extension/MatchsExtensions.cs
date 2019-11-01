/* Ceated by Ya Lin. 2019/10/28 16:55:23 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.Standard.Lib.Extension
{
    public static class MatchsExtensions
    {
        /// <summary>
        /// 获取满足合计的组合值
        /// </summary>
        /// <param name="src">源数组</param>
        /// <param name="targetResult">合计值</param>
        /// <param name="maxCount">最大组合数（默认：3）</param>
        /// <returns></returns>
        public static int[] MatchSum(this List<int> src, int targetResult, int maxCount = 3)
            => src.ToArray().MatchSum(targetResult, maxCount);

        /// <summary>
        /// 获取满足合计的组合值
        /// </summary>
        /// <param name="src">源数组</param>
        /// <param name="targetResult">合计值</param>
        /// <param name="maxCount">最大组合数（默认：3）</param>
        /// <returns></returns>
        public static int[] MatchSum(this int[] src, int targetResult, int maxCount = 3)
        {
            var mlist = src.Where(x => x <= targetResult).ToList();
            var result = mlist.ToArray().GetCombine(maxCount, 1, x => x.Sum() == targetResult, true);
            var r = result?.FirstOrDefault();
            return r;
        }

        /// <summary>
        /// 获取满足合计的组合值
        /// </summary>
        /// <param name="src">源数组</param>
        /// <param name="targetResult">合计值</param>
        /// <param name="maxCount">最大组合数（默认：3）</param>
        /// <returns></returns>
        public static int[][] MatchSumAll(this int[] src, int targetResult, int maxCount = 3)
        {
            var mlist = src.Where(x => x <= targetResult).ToList();
            var result = mlist.ToArray().GetCombine(maxCount, 1, x => x.Sum() == targetResult);
            return result;
        }

        /// <summary>
        /// 获取所有满足合计的组合值
        /// </summary>
        /// <param name="src">源数组</param>
        /// <param name="targetResult">合计值</param>
        /// <param name="maxCount">最大组合数（默认：3）</param>
        /// <returns></returns>
        public static int[][] MatchSumAll(this List<int> src, int targetResult, int maxCount = 3)
            => src.ToArray().MatchSumAll(targetResult, maxCount);

        /// <summary>
        /// 获取指定长度的组合数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">源数组</param>
        /// <param name="maxCount">最大组合长度</param>
        /// <param name="minCount">最小组合长度（默认为1）</param>
        /// <param name="predicate">组合条件（默认返回所有组合）</param>
        /// <param name="returnByFirst">仅获取第一组满足条件的组合</param>
        /// <returns></returns>
        public static T[][] GetCombine<T>(this T[] src, int maxCount, int minCount = 1, Predicate<T[]> predicate=null, bool returnByFirst = false)
        {
            var result = new List<T[]>();
            minCount = minCount <= 0 ? 1 : minCount;

            for (int i = minCount-1; i < maxCount; i++)
            {
                var remainList = new List<T>(src);
                foreach (var item in src)
                {
                    remainList.Remove(item);
                    var sr = new[] { item };
                    var s = sr.GetCombine(remainList.ToArray(), i + 1, predicate, returnByFirst);
                    if (s != null)
                    {
                        result.AddRange(s);
                    }
                    if (returnByFirst && result.Count > 0)
                    {
                        return result.ToArray();
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 获取指定长度的组合数组
        /// </summary>
        /// <typeparam name="T">数组对象</typeparam>
        /// <param name="srca">源数组</param>
        /// <param name="srcb">待选数组</param>
        /// <param name="count">组合个数</param>
        /// <param name="predicate">组合条件</param>
        /// <param name="returnByFirst">是否符合条件就停止</param>
        /// <returns></returns>
        private static T[][] GetCombine<T>(this T[] srca, T[] srcb, int count, Predicate<T[]> predicate = null, bool returnByFirst = false)
        {
            var result = new List<T[]>();
            if (srca.Length >= count)
            {
                if (predicate?.Invoke(srca) ?? false) result.Add(srca);
                return result.ToArray();
            }
            var listB = new List<T>(srcb);
            for (int i = 0; i < srcb.Length; i++)
            {
                var listA = new List<T>(srca);
                listA.Add(srcb[i]);
                listB.Remove(srcb[i]);
                var s = listA.ToArray().GetCombine<T>(listB.ToArray(), count, predicate);
                result.AddRange(s);
                if (returnByFirst && result.Count > 0)
                {
                    return result.ToArray();
                }
            }
            return result.ToArray();
        }


    }
}
