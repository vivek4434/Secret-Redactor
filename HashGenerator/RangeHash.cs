namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.Hash.Generator.Enums;
    using Secret.Redactor.Hash.Generator.Interfaces;
    using System;

    /// <summary>
    /// Provides the abstraction for calculation of hashes of a given range.
    /// </summary>
    internal class RangeHash : IRangeHash
    {
        private readonly long mod = 1000000007;

        private long[] prefixSumFromA;

        private long[] prefixSumFromB;

        private IPowerModule powerModule;
        private readonly string text;

        internal RangeHash(IPowerModule powerModule, string text)
        {
            this.prefixSumFromA = null;
            this.prefixSumFromB = null;
            this.text = text;
            this.powerModule = powerModule;

            this.PopulatePrefixHashSum();
        }

        public long GetRangeHash(int start, int end, HashType hashType)
        {
            // this gives hash for [start, end].
            switch (hashType)
            {
                case HashType.HashTypeA:
                    return this.GetTextHashFromTypeA(start, end);
                case HashType.HashTypeB:
                    return this.GetTextHashFromTypeB(start, end);
                default:
                    throw new InvalidOperationException("Unsupported hash type.");
            }
        }

        private void PopulatePrefixHashSum()
        {
            int len = text.Length;
            Array.Resize(ref prefixSumFromA, len);
            Array.Resize(ref prefixSumFromB, len);

            prefixSumFromA[0] = (text[0] * powerModule.GetPower(0, HashType.HashTypeA)) % mod;
            prefixSumFromB[0] = (text[0] * powerModule.GetPower(0, HashType.HashTypeB)) % mod;

            for (int i = 1; i < len; i++)
            {
                prefixSumFromA[i] = (prefixSumFromA[i] + text[i] * powerModule.GetPower(0, HashType.HashTypeA)) % mod;
                prefixSumFromB[i] = (prefixSumFromB[i] + text[i] * powerModule.GetPower(0, HashType.HashTypeB)) % mod;
            }
        }

        private long GetTextHashFromTypeA(int start, int end)
        {
            // this gives hash for [start, end].
            if (start <= 0)
            {
                return (prefixSumFromA[end] * powerModule.GetInvPower(start, HashType.HashTypeA)) % mod;
            }

            // ((pj - pi)^base^-i)
            return ((mod + prefixSumFromA[end] - prefixSumFromA[start - 1]) % mod) * powerModule.GetInvPower(start, HashType.HashTypeA) % mod;
        }

        private long GetTextHashFromTypeB(int start, int end)
        {
            // this gives hash for [start, end].
            if (start <= 0)
            {
                return (prefixSumFromB[end] * powerModule.GetInvPower(start, HashType.HashTypeB)) % mod;
            }

            // ((pj - pi)^base^-i)
            return ((mod + prefixSumFromB[end] - prefixSumFromB[start - 1]) % mod) * powerModule.GetInvPower(start, HashType.HashTypeB) % mod;
        }
    }
}
