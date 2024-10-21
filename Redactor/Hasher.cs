namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.TwoPointer;
    using Secret.Redactor.TwoPointer.Interfaces;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class Hasher : IHasher
    {
        private readonly uint maxPowerPreComputationLength;
        private readonly HashSet<long> hashes31;
        private readonly HashSet<long> hashes257;
        private readonly ConcurrentDictionary<int, long> powers31;
        private readonly ConcurrentDictionary<int, long> powers257;
        private readonly ConcurrentDictionary<char, bool> secretCharacters;

        public Hasher(uint maxPowerPreComputationLength = 100)
        {
            this.maxPowerPreComputationLength= maxPowerPreComputationLength;
            this.hashes31 = new HashSet<long>();
            this.hashes257 = new HashSet<long>();
            this.powers31 = new ConcurrentDictionary<int, long>();
            this.powers257 = new ConcurrentDictionary<int, long>();
            this.secretCharacters = new ConcurrentDictionary<char, bool>();
            PrecomputeInitialPowers();
        }

        public bool Match(long hash31, long hash257)
        {
            return hashes31.Contains(hash31) && hashes257.Contains(hash257);
        }

        public (long, long) UpdateHashes(char c, long hash31, long hash257)
        {
            hash31 = (hash31 * 31 + c) % Constants.Mod;
            hash257 = (hash257 * 257 + c) % Constants.Mod;
            return (hash31, hash257);
        }

        public void GenerateHashes(IEnumerable<string> secrets)
        {
            foreach (var secret in secrets)
            {
                long hash31 = 0;
                long hash257 = 0;

                for (int i = 0; i < secret.Length; i++)
                {
                    char c = secret[i];
                    hash31 = (hash31 + c * GetPower31(secret.Length - 1 - i)) % Constants.Mod;
                    hash257 = (hash257 + c * GetPower257(secret.Length - 1 - i)) % Constants.Mod;
                }

                hashes31.Add(hash31);
                hashes257.Add(hash257);
            }
        }
        public void UpdateSecretCharacters(IEnumerable<char> newSecretCharacters)
        {
            foreach (var c in newSecretCharacters)
            {
                this.secretCharacters.TryAdd(c, true);
            }
        }

        public bool IsSecretCharacter(char c)
        {
            return this.secretCharacters.ContainsKey(c);
        }

        private long GetPower31(int exponent)
        {
            if (!powers31.ContainsKey(exponent))
            {
                long previousPower = GetPower31(exponent - 1);
                long newPower = (previousPower * 31) % Constants.Mod;
                powers31[exponent] = newPower;
            }
            return powers31[exponent];
        }

        private long GetPower257(int exponent)
        {
            if (!powers257.ContainsKey(exponent))
            {
                long previousPower = GetPower257(exponent - 1);
                long newPower = (previousPower * 257) % Constants.Mod;
                powers257[exponent] = newPower;
            }
            return powers257[exponent];
        }

        private void PrecomputeInitialPowers()
        {
            powers31[0] = 1;
            powers257[0] = 1;
            for (int i = 1; i <= this.maxPowerPreComputationLength; i++)
            {
                powers31[i] = (powers31[i - 1] * 31) % Constants.Mod;
                powers257[i] = (powers257[i - 1] * 257) % Constants.Mod;
            }
        }
    }
}
