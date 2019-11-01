using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thunder.Standard.Lib.Extension;
/* Ceated by Ya Lin. 2019/11/1 14:10:51 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Thunder.Standard.Lib.Extension.Tests
{
    [TestClass()]
    public class MatchsExtensionsTests
    {
        [TestMethod]
        public void i01_MatchSum()
        {
            var list = new List<int> { 25000, 1200, 1900, 800, 300, 650, 20, 1600, 400 };
            var result = list.MatchSum(2200);
            var a = new[] { 1900, 300 };
            var r = result.Except(a).ToList();
            Assert.IsTrue(r.Count() == 0);

            result = list.MatchSum(1600);
            a = new[] { 1600 };
            r = result.Except(a).ToList();
            Assert.IsTrue(r.Count() == 0);

            result = list.MatchSum(2650);
            a = new[] { 1200, 800, 650 };
            r = result.Except(a).ToList();
            Assert.IsTrue(r.Count() == 0);

            result = list.MatchSum(2670);
            Assert.IsNull(result);

            result = list.MatchSum(2670, 5);
            a = new[] { 1200, 800, 650, 20 };
            r = result.Except(a).ToList();
            Assert.IsTrue(r.Count() == 0);
        }

        [TestMethod]
        public void i02_MatchSumAll()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6 };

            var result = list.MatchSumAll(10, 4);
            Assert.IsTrue(result.Length == 5);

            result = list.ToArray().GetCombine(3, 3, x => x.Sum() == 9);
            Assert.IsTrue(result.Length == 3);
        }

        //[TestMethod]
        public void i03_性能测试()
        {
            //var list = new int[50];
            //for (int i = 0; i < list.Length; i++)
            //{
            //    list[i] = i + 1;
            //}

            var list = new[] { 12000, 9000, 10000, 18000, 32000, 24000, 20000, 1200, 600, 2480, 2750, 1650, 2450, 1950, 800, 300, 500, 1000, 100, 1600, 1900, 796, 800, 650, 20, 200 };

            //var r = list.GetCombine(5);
            var r = list.MatchSumAll(27120, 10);
            r = list.MatchSumAll(32570, 10);
            r = list.MatchSumAll(34416, 10);
            r = list.MatchSumAll(18716, 10);
            r = list.MatchSumAll(34420, 10);
            System.Console.WriteLine(r.Length);
        }
    }
}