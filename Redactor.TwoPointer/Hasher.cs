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
        private readonly ConcurrentDictionary<char, bool> secretCharacters;
        private readonly IReadOnlyDictionary<long, long> invMod;
        private long power31 = 1; //31
        private long power257 = 1; // 257
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Hasher"/> class.
        /// </summary>
        /// <param name="maxPowerPreComputationLength">The maximum length for precomputing powers.</param>
        public Hasher(uint maxPowerPreComputationLength = 40)
        {
            this.maxPowerPreComputationLength= maxPowerPreComputationLength;
            this.hashes31 = new HashSet<long>();
            this.hashes257 = new HashSet<long>();
            this.secretCharacters = new ConcurrentDictionary<char, bool>();
            invMod = new Dictionary<long, long>()
            {
                [Constants.PrimaryPrime] = this.CalculateModularInverse(Constants.PrimaryPrime, Constants.Mod),
                [Constants.SecondaryPrime] = this.CalculateModularInverse(Constants.SecondaryPrime, Constants.Mod)
            };
        }

        public void Reset()
        {
            this.power257 = 1;
            this.power31 = 1;
        }

        /// <inheritdoc/>
        public bool Match(long hash31, long hash257)
        {
            return hashes31.Contains(hash31) && hashes257.Contains(hash257);
        }

        /// <inheritdoc/>
        public (long, long) UpdateHashes(char c, long hash31, long hash257)
        {
            hash31 = (hash31 * Constants.PrimaryPrime + c) % Constants.Mod;
            hash257 = (hash257 * Constants.SecondaryPrime + c) % Constants.Mod;
            power31 = this.Multiply(power31, Constants.PrimaryPrime); 
            power257 = this.Multiply(power257 * Constants.SecondaryPrime);
            return (hash31, hash257);
        }

        /// <inheritdoc/>
        public (long, long) RemoveHash(char character, long hash31, long hash257)
        {
            // ((currentHash - character*baseValue^exponent + mod)*base^-1)%mod;
            hash31 = (hash31 - this.Multiply(character, power31) + Constants.Mod) % Constants.Mod;
            hash257 = (hash257 - this.Multiply(character, power257) + Constants.Mod) % Constants.Mod;
            power31 /= Constants.PrimaryPrime;
            power257 /= Constants.SecondaryPrime;

            return (hash31, hash257);
        }

        /// <inheritdoc/>
        public void GenerateHashes(IEnumerable<string> secrets)
        {
            foreach (var secret in secrets)
            {
                for (int i = 0; i < secret.Length; i++)
                {
                    long hash31 = 0;
                    long hash257 = 0;

                    this.secretCharacters.TryAdd(secret[i], true);
                    for (int j = i; j < secret.Length; j++)
                    {
                        char c = secret[j];
                        hash31 = (hash31 * Constants.PrimaryPrime + c) % Constants.Mod;
                        hash257 = (hash257 * Constants.SecondaryPrime + c) % Constants.Mod;
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

        private long Multiply(long a, long b) 
        {
            if (a > Constants.Mod)
            {
                a %= Constants.Mod;
            }

            if (b > Constants.Mod)
            {
                b %= Constants.Mod;
            }

            return (a * b) % Constants.Mod;
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
