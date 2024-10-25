namespace Secret.Redactor.TwoPointer.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Secret.Redactor.TwoPointer.Extensions;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class TestStringMasking
    {
        [TestMethod]
        public void Test_When_NullOrEmpty_Intervals()
        {
            string input = "Hello secret redactor";
            var interval = new List<(int start, int end)>();
            Assert.IsTrue(string.Equals(input.MaskString(interval), input));

            // When interval array is null.
            Assert.IsTrue(string.Equals(input.MaskString(null), input));
        }

        [TestMethod]
        public void Test_When_All_Intervals_Are_Less_Than_3()
        {
            string input = "Hello secret redactor";
            var interval = new List<(int start, int end)>()
            {
                (0, 1),
                (1, 3),
                (3, 5)
            };

            Assert.IsTrue(string.Equals(input.MaskString(interval), input));

            // When interval array is null.
            Assert.IsTrue(string.Equals(input.MaskString(null), input));
        }

        [TestMethod]
        public void Test_Mixed_Interval_Set()
        {
            string input = "Hello secret redactor";
            var interval = new List<(int start, int end)>()
            {
                (0, 3),
                (3, 5),
                (5, 6)
            };

            Assert.IsTrue(string.Equals(input.MaskString(interval), "$PASSlo secret redactor"));
        }

        [TestMethod]
        public void Test_FullString_Masking()
        {
            string input = "Hello secret redactor";
            var interval = new List<(int start, int end)>()
            {
                (0, input.Length)
            };

            Assert.IsTrue(string.Equals(input.MaskString(interval), "$PASS"));
        }
    }
}
