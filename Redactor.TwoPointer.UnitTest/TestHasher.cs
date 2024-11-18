using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 4, Scope = ExecutionScope.MethodLevel)]

namespace Secret.Redactor.TwoPointer.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Secret.Redactor.Hash.Generator;
    using System.Collections.Generic;

    [TestClass]
    public class TestHasher
    {
        [TestMethod]
        public void Constructor_ShouldInitializeFields()
        {
            var hasher = new Hasher();

            Assert.IsNotNull(hasher);
        }

        [TestMethod]
        public void Match_ShouldReturnTrue_WhenHashesExist()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(new List<string> { "secret" });
            long power31 = 1;
            long power257 = 1;
            (var hash31, var hash257, power31, power257) = 
                hasher.UpdateHashes('s', 0, 0, power31, power257);

            Assert.IsTrue(hasher.Match(hash31, hash257));
        }

        [TestMethod]
        public void UpdateHashes_ShouldReturnUpdatedHashes()
        {
            var hasher = new Hasher();
            long power31 = 1;
            long power257 = 1;
            
            (var hash31, var hash257, power31, power257) = 
                hasher.UpdateHashes('a', 0, 0, power31, power257);

            Assert.AreNotEqual<long>(0, hash31);
            Assert.AreNotEqual<long>(0, hash257);
        }

        [TestMethod]
        public void RemoveHash_ShouldReturnUpdatedHash()
        {
            var hasher = new Hasher();
            long power31 = 1;
            long power257 = 1;

            (long hash31, long hash257, power31, power257) = 
                hasher.UpdateHashes('a', 0, 0, power31, power257);
            (long updatedHash31, long updatedHash257, power31, power257) 
                = hasher.RemoveHash('a', hash31, hash257, power31, power257);

            Assert.AreNotEqual<long>(hash31, updatedHash31);
            Assert.AreNotEqual<long>(hash257, updatedHash257);
        }

        [TestMethod]
        public void GenerateHashes_ShouldPopulateHashes()
        {
            var hasher = new Hasher();
            long power31 = 1;
            long power257 = 1;

            hasher.GenerateHashes(new List<string> { "secret" });

            (var hash31, var hash257, power31, power257) = 
                hasher.UpdateHashes('s', 0, 0, power31, power257);

            Assert.IsTrue(hasher.Match(hash31, hash257));
        }

        [TestMethod]
        public void UpdateSecretCharacters_ShouldAddNewCharacters()
        {
            var hasher = new Hasher();
            hasher.UpdateSecretCharacters(new List<char> { 'a', 'b', 'c' });

            Assert.IsTrue(hasher.IsSecretCharacter('a'));
            Assert.IsTrue(hasher.IsSecretCharacter('b'));
            Assert.IsTrue(hasher.IsSecretCharacter('c'));
            Assert.IsFalse(hasher.IsSecretCharacter(' '));
        }

        [TestMethod]
        public void IsSecretCharacter_ShouldReturnTrue_WhenCharacterExists()
        {
            var hasher = new Hasher();
            hasher.UpdateSecretCharacters(new List<char> { 'a' });

            Assert.IsTrue(hasher.IsSecretCharacter('a'));
        }

        [TestMethod]
        public void Match_ShouldReturnFalse_WhenHashesDoNotExist()
        {
            var hasher = new Hasher();
            var result = hasher.Match(12345, 67890);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Match_ShouldReturnTrue_ForSubstringHashes()
        {
            // Arrange
            var hasher = new Hasher();
            var secrets = new List<string> { "secret1", "secret2" };
            hasher.GenerateHashes(secrets);

            var substring = "cret";
            long hash31 = 0;
            long hash257 = 0;
            long power31 = 1;
            long power257 = 1;
            foreach (var c in substring)
            {
                (hash31, hash257, power31, power257) = 
                    hasher.UpdateHashes(c, hash31, hash257, power31, power257);
            }

            var result = hasher.Match(hash31, hash257);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveHash_ShouldReturnCorrectHash_WhenFirstCharRemoved()
        {
            var hasher = new Hasher();
            var initialHash31 = 0L;
            var initialHash257 = 0L;
            long power31 = 1;
            long power257 = 1;

            foreach (var c in "abcdefg")
            {
                (initialHash31, initialHash257, power31, power257) = 
                    hasher.UpdateHashes(c, initialHash31, initialHash257, power31, power257);
            }

            // hashes after remove of a.
            (long updatedHash31, long updatedHash257, power31, power257) = 
                hasher.RemoveHash('a', initialHash31, initialHash257, power31, power257);

            (var targetHash31, var targetHash257) = (0L, 0L);
            foreach (var c in "bcdefg")
            {
                (targetHash31, targetHash257, power31, power257) = 
                    hasher.UpdateHashes(c, initialHash31, initialHash257, power31, power257);
            }

            Assert.AreNotEqual<long>(updatedHash31, targetHash31);
            Assert.AreNotEqual<long>(updatedHash257, targetHash257);
        }
    }
}
