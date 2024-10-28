namespace Secret.Redactor.TwoPointer.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Secret.Redactor.Hash.Generator;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;

    [TestClass]
    public class TestHasherPerformance
    {
        // Singleton instance.
        private readonly int maxSecretGenCount = 100000;
        private Hasher hasher;
        private ConcurrentBag<string> secrets;
        private static readonly char[] Characters = "~`.,';:-}{?<>+ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()".ToCharArray();

        [TestInitialize]
        public void Setup()
        {
            hasher = new Hasher(40);
            secrets = new ConcurrentBag<string>();
            var random = new Random();

            for (int i = 0; i < maxSecretGenCount; i++)
            {
                secrets.Add(this.GeneratePassword(random.Next(10, 30)));
            }
        }

        [TestMethod]
        public void TestGenerateHashesPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            hasher.GenerateHashes(secrets);
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 8000, $"GenerateHashes took {stopwatch.ElapsedMilliseconds}");
        }

        [TestMethod]
        public void TestUpdateHashesPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var secret in secrets)
            {
                var(hash31, hash257) = (0L, 0L);
                foreach (var c in secret)
                {
                    (hash31, hash257) = hasher.UpdateHashes(c, hash31, hash257);
                }
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 60, $"UpdateHashes() took: {stopwatch.ElapsedMilliseconds}");
        }

        [TestMethod]
        public void TestMatchPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var secret in secrets)
            {
                var (hash31, hash257) = (0L, 0L);
                foreach (var c in secret)
                {
                    (hash31, hash257) = hasher.UpdateHashes(c, hash31, hash257);
                    _ = hasher.Match(hash31, hash257);
                }
            }
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 100, $"Match took: {stopwatch.ElapsedMilliseconds}");
        }

        [TestMethod]
        public void TestRemoveHashPerformance()
        {
            var random = new Random();
            // Precompute hashes for the secrets
            var hashPairs = new List<(long, long)>();
            foreach (var secret in secrets)
            {
                long hash31 = 0;
                long hash257 = 0;
                foreach (var c in secret)
                {
                    (hash31, hash257) = hasher.UpdateHashes(c, hash31, hash257);
                }
                hashPairs.Add((hash31, hash257));
            }

            var secretCopy = secrets.ToArray();

            // Measure RemoveHash performance under stress
            var stopwatch = Stopwatch.StartNew();
            foreach (var (hash31, hash257) in hashPairs)
            {
                int index = random.Next(secretCopy.Length);
                for (int c =  0; c < secretCopy[index].Length; c++)
                {
                    var newHash31 = hasher.RemoveHash(secretCopy[index][c], hash31, 31, secretCopy[index].Length - c - 1);
                    var newHash257 = hasher.RemoveHash(secretCopy[index][c], hash257, 257, secretCopy[index].Length - c - 1);
                }
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 400, $"Match took: {stopwatch.ElapsedMilliseconds}");
        }

        private string GeneratePassword(int length)
        {
            var password = new StringBuilder();
            var data = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            for (int i = 0; i < length; i++)
            {
                var index = data[i] % Characters.Length;
                password.Append(Characters[index]);
            }

            return password.ToString();
        }
    }
}
