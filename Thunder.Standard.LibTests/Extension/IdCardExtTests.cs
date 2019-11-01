using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thunder.Standard.Lib.Extension;
/* Ceated by Ya Lin. 2019/11/1 14:13:09 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.Standard.Lib.Extension.Tests
{
    [TestClass()]
    public class IdCardExtTests
    {
        [TestMethod()]
        public void IsIdCardTest()
        {
            Assert.IsTrue(!"1000000000000000000".IsIdCard(out var dat));
            Assert.IsTrue("511022200212013335".IsIdCard(out dat));
        }
    }
}