namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.TwoPointer;
    using Secret.Redactor.TwoPointer.Interfaces;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Hasher class for generating and managing hash values for secret redaction.
    /// </summary>
    public class Hasher : IHasher
    {
        private readonly uint maxPowerPreComputationLength;
        private readonly HashSet<long> hashes31;
        private readonly HashSet<long> hashes257;
        private readonly ConcurrentDictionary<int, long> powers31;
        private readonly ConcurrentDictionary<int, long> powers257;
        private readonly ConcurrentDictionary<char, bool> secretCharacters;
        private readonly IReadOnlyDictionary<long, long> invMod;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hasher"/> class.
        /// </summary>
        /// <param name="maxPowerPreComputationLength">The maximum length for precomputing powers.</param>
        public Hasher(uint maxPowerPreComputationLength = 40)
        {
            this.maxPowerPreComputationLength= maxPowerPreComputationLength;
            this.hashes31 = new HashSet<long>();
            this.hashes257 = new HashSet<long>();
            this.powers31 = new ConcurrentDictionary<int, long>();
            this.powers257 = new ConcurrentDictionary<int, long>();
            this.secretCharacters = new ConcurrentDictionary<char, bool>();
            invMod = new Dictionary<long, long>()
            {
                [31] = this.CalculateModularInverse(31, Constants.Mod),
                [257] = this.CalculateModularInverse(257, Constants.Mod)
            };

            PrecomputeInitialPowers();
        }

        /// <inheritdoc/>
        public bool Match(long hash31, long hash257)
        {
            return hashes31.Contains(hash31) && hashes257.Contains(hash257);
        }

        /// <inheritdoc/>
        public (long, long) UpdateHashes(char c, long hash31, long hash257)
        {
            hash31 = (hash31 * 31 + c) % Constants.Mod;
            hash257 = (hash257 * 257 + c) % Constants.Mod;
            return (hash31, hash257);
        }

        /// <inheritdoc/>
        public long RemoveHash(char character, long currentHashValue, int baseValue)
        {
            // ((currentHash - character + mod)*base^-1)%mod;

            // Adjust the hash value by subtracting the character's value and adding the modulus to ensure it remains positive
            currentHashValue = (currentHashValue - character + Constants.Mod) % Constants.Mod;

            // Multiply the result by the modular inverse of the base value to correctly update the hash value
            currentHashValue = (currentHashValue * invMod[baseValue]) % Constants.Mod;

            return currentHashValue;
        }

        /// <inheritdoc/>
        public void GenerateHashes(IEnumerable<string> secrets)
        {
            foreach (var secret in secrets)
            {
                long hash31 = 0;
                long hash257 = 0;

                for (int i = 0; i < secret.Length; i++)
                {
                    this.secretCharacters.TryAdd(secret[i], true);
                    for (int j = i; j < secret.Length; j++)
                    {
                        char c = secret[i];
                        hash31 = (hash31 + c * GetPower31(secret.Length - 1 - i)) % Constants.Mod;
                        hash257 = (hash257 + c * GetPower257(secret.Length - 1 - i)) % Constants.Mod;
                        hashes31.Add(hash31);
                        hashes257.Add(hash257);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void UpdateSecretCharacters(IEnumerable<char> newSecretCharacters)
        {
            foreach (var c in newSecretCharacters)
            {
                this.secretCharacters.TryAdd(c, true);
            }
        }

        /// <inheritdoc/>
        public bool IsSecretCharacter(char c)
        {
            return this.secretCharacters.ContainsKey(c);
        }

        /// <summary>
        /// Gets the power of 31 for the given exponent.
        /// </summary>
        /// <param name="exponent">The exponent.</param>
        /// <returns>The power of 31.</returns>
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

        /// <summary>
        /// Gets the power of 257 for the given exponent.
        /// </summary>
        /// <param name="exponent">The exponent.</param>
        /// <returns>The power of 257.</returns>
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

        /// <summary>
        /// Precomputes the initial powers of 31 and 257 up to the specified maximum length.
        /// </summary>
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

        /// <summary>
        /// Computes the modular inverse of a number using the extended Euclidean algorithm.
        /// </summary>
        /// <param name="number">The number to find the inverse of.</param>
        /// <param name="modulus">The modulus.</param>
        /// <returns>The modular inverse of the number.</returns>
        private long CalculateModularInverse(long number, long modulus)
        {
            long originalModulus = modulus, temporaryValue, quotient;
            long x0 = 0, x1 = 1;
            if (modulus == 1)
                return 0;

            // Apply the extended Euclidean algorithm
            while (number > 1)
            {
                quotient = number / modulus;

                temporaryValue = modulus;
                modulus = number % modulus;
                number = temporaryValue;

                temporaryValue = x0;

                x0 = x1 - quotient * x0;

                x1 = temporaryValue;
            }

            if (x1 < 0)
                x1 += originalModulus;

            return x1;
        }
    }
}
