namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.Hash.Generator.Enums;
    using Secret.Redactor.Hash.Generator.Interfaces;
    using System;

    /// <summary>
    /// Power utility to provide P^i or P^(-i).
    /// </summary>
    internal class PowerModule : IPowerModule
    {
        private const uint powersLength = 1012;
        private readonly long[] powersA = new long[powersLength];
        private readonly long[] invPowersA = new long[powersLength];

        private readonly long[] powersB = new long[powersLength];
        private readonly long[] invPowersB = new long[powersLength];

        private readonly long baseNumA = 31;

        private readonly long baseNumB = 257;

        private readonly long mod = 1000000007;

        internal PowerModule() 
        {
            this.populatePowers();
            this.populateInvPowers();
        }

        public long GetPower(int power, HashType hashType)
        {
            // this gives hash for [start, end].
            switch (hashType)
            {
                case HashType.HashTypeA:
                    return powersA[power];
                case HashType.HashTypeB:
                    return powersB[power];
                default:
                    throw new InvalidOperationException("Unsupported hash type.");
            }
        }

        public long GetInvPower(int power, HashType hashType)
        {
            // this gives hash for [start, end].
            switch (hashType)
            {
                case HashType.HashTypeA:
                    return invPowersA[power];
                case HashType.HashTypeB:
                    return invPowersB[power];
                default:
                    throw new InvalidOperationException("Unsupported hash type.");
            }
        }

        private void populatePowers()
        {
            long currValueA = 1, currValueB = 1;
            for (int i = 0; i < powersLength; i++)
            {
                this.powersA[i] = currValueA;
                currValueA = (currValueA * baseNumA) % mod;

                this.powersB[i] = currValueB;
                currValueB = (currValueB * baseNumB) % mod;
            }
        }

        private void populateInvPowers()
        {
            invPowersA[0] = 1;
            invPowersA[1] = 1;

            invPowersB[0] = 1;
            invPowersB[1] = 1;

            for (int i = 2; i < powersLength; i++)
            {
                invPowersA[i] = mod - (long)(mod / i) * invPowersA[mod % i] % mod;

                invPowersB[i] = mod - (long)(mod / i) * invPowersB[mod % i] % mod;
            }
        }
    }
}