using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thunder.Standard.Lib.Extension;
/* Ceated by Ya Lin. 2019/11/1 14:14:15 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.Standard.Lib.Extension.Tests
{
    [TestClass()]
    public class StringExtTests
    {
        [TestMethod()]
        public void LikeTest_layout_0()
        {
            var src = "何思琪";
            var key = "何思_";
            Assert.IsTrue(src.Like(key, 0));
        }

        [TestMethod()]
        public void LikeTest_layout_1()
        {
            var src = "何思琪";
            var key = "何思淇";
            Assert.IsTrue(src.Like(key));
            Assert.IsTrue("曾扬".Like("曾洋"));
        }

        [TestMethod()]
        public void LikeTest_layout_2_more()
        {
            var src = "何思琪";
            var key = "何思淇";
            Assert.IsTrue(src.Like(key, 2));
        }

        [TestMethod()]
        public void LikeTest_null()
        {
            var src = "";
            var key = "何思淇";
            Assert.IsFalse(src.Like(key));
            Assert.IsFalse(src.Like(""));
        }

        [TestMethod()]
        public void LikeTest_Notmatch()
        {
            var src = "曾扬剑";
            var key = "何思淇";
            Assert.IsFalse(src.Like(key));
        }

        [TestMethod()]
        public void LikeTest_layout_2_False()
        {
            var src = "曾扬剑候东霖";
            var key = "何思淇侯东霖";
            Assert.IsFalse(src.Like(key, 2));
            Assert.IsFalse(src.Like("扬贱厚东霖", 2));
        }

        [TestMethod()]
        public void LikeTest_layout_2_True()
        {
            var src = "曾扬剑候东霖";
            Assert.IsTrue(src.Like("扬贱厚东霖", 2, true));
        }

        [TestMethod()]
        public void LikeTest_layout_3_True()
        {
            var src = "扬剑候东霖";
            Assert.IsTrue(src.Like("羊剑候冬林", 3));
        }

        [TestMethod()]
        public void LikeTest_keylen_1_false()
        {
            var src = "扬剑候东霖";
            Assert.IsFalse(src.Like("扬", 1, true));
        }

        [TestMethod()]
        public void LikeTest_keymaxlen_false()
        {
            var src = "扬剑候东霖张栋王";
            Assert.IsTrue(src.Like("候东霖帐栋", 1, true));
            Assert.IsTrue(src.Like("扬剑候东霖张栋王"));
            Assert.IsFalse(src.Like("扬剑候东霖帐栋", 1, true));
            Assert.IsFalse(src.Like("扬剑候东霖张栋"));
        }

        [TestMethod()]
        public void LikeTest_Idcard()
        {
            Assert.IsTrue("51343420120807181X".Like("51343420120807181x"));
            Assert.IsTrue("51343420120807181X".Like("3420120807181x", 0, true));
            Assert.IsTrue("51343420120807181X".Like("20110817", 2, true, 8));
        }

        [TestMethod()]
        public void KeyLikeTest_All()
        {
            Assert.IsTrue("曾扬剑".KeyLike("曾扬剑"));
            Assert.IsTrue("曾扬剑".KeyLike("曾_剑"));
            Assert.IsTrue("曾扬剑".KeyLike("曾扬_"));
            Assert.IsTrue("曾扬剑".KeyLike("曾__"));
            Assert.IsTrue("曾扬剑".KeyLike("___"));
        }

        [TestMethod()]
        public void KeyLikeTest_full_false()
        {
            Assert.IsFalse("曾扬剑".KeyLike("_"));
            Assert.IsFalse("曾扬剑".KeyLike("曾_"));
        }

        [TestMethod()]
        public void KeyLikeTest_part_true()
        {
            Assert.IsTrue("曾扬剑".KeyLike("_", true));
            Assert.IsTrue("曾扬剑".KeyLike("曾_", true));
        }

        [TestMethod()]
        public void KeyLikeTest_Null()
        {
            Assert.IsFalse("".KeyLike("_"));
            Assert.IsFalse("曾扬剑".KeyLike(null));
        }
    }
}