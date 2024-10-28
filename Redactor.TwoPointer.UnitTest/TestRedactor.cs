namespace Secret.Redactor.TwoPointer.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using Secret.Redactor.Hash.Generator;
    using System;

    [TestClass]
    public class TestRedactor
    {
        private const string TEST_COMMAND_1 = " key 123400000000000000000000000000000000000000000000abc\n\n\n\n\n\n\nRP/0/RP0/CPU(config-test-primary-test-64309)#";
        private readonly IEnumerable<string> TEST_LOOKUP_SET = new List<string>() { "TEST_SECRET_1", "TEST_SECRET_2", "123400000000000000000000000000000000000000000000abc", "12340000678", "ion", "678", "o9-*t&p()(" };
        private const string TEST_COMMAND = "This version\n\n\n\n versions is a TEST_SECRET_1";
        private const string TEST_COMMAND_2 = "12340000\n678\ndummydata";
        private const string TEST_COMMAND_3 = "This is a sample log. Password: 1234000\n000000abc678";
        private const string TEST_COMMAND_4 = "This is a string that contains a password with special characters. Admin:o9-*t&p()(";
        private const string TEST_SECRET_1 = "TEST_SECRET_1";
        private const string TEST_SECRET_2 = "TEST_SECRET_2";
        private const string TEST_SECRET_3 = "123400000000000000000000000000000000000000000000abc";
        private const string TEST_SECRET_4 = "678";
        private const string TEST_SECRET_5 = "o9-*t&p()(";

        [TestMethod]
        public void TestRedactSuccess()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND);
            Assert.IsTrue(response.StartsWith("This vers$PASS$"));
            Assert.IsTrue(response.EndsWith("vers$PASS$s is a $PASS$"));
        }

        [TestMethod]
        public void TestRedactSuccessWithNewLine()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND_1);
            Assert.IsTrue(response.Contains(" key $PASS$"));
        }

        [TestMethod]
        public void TestRedactSuccessPartialSecret()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND_2);
            Assert.IsTrue(response.Contains("$PASS$\n$PASS$\ndummydata"));
        }

        [TestMethod]
        public void TestRedactSuccessAdditionalInput()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND_2);
            Assert.IsTrue(response.Contains("$PASS$\n$PASS$\ndummydata"));
        }

        [TestMethod]
        public void TestRedactSuccessAdditionalInputWithEventGuid()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND_3);
            Console.WriteLine(response);
            Assert.IsTrue(response.Contains("This is a sample log. Password: $PASS$\n$PASS$$PASS$"));
        }

        public void TestRedactSuccessAdditionalInputWithEventGuidAndPasswordWithSpecialCharacters()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            string response = new Redactor(hasher).Redact(TEST_COMMAND_4);
            Assert.IsTrue(!response.Contains(TEST_SECRET_5));
        }

        [TestMethod]
        public void TestRedactFailureNullInput()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(TEST_LOOKUP_SET);
            var response = new Redactor(hasher).Redact(null);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void TestRedactSuccessWithZeroRedaction()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(new HashSet<string> { TEST_SECRET_4 });
            var response = new Redactor(hasher).Redact(TEST_COMMAND);
            Assert.IsTrue(response.Contains("TEST_SECRET"));
        }
    }
}
