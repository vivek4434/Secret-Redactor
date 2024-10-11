namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.Hash.Generator.Enums;
    using Secret.Redactor.Hash.Generator.Interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// Zeni is hash manager for all texts (and substrings) that
    /// one may query.
    /// </summary>
    public class Zeni
    {
        private readonly IEnumerable<string> bagOfText;
        private readonly HashSet<long> bagOfHashesA;
        private readonly HashSet<long> bagOfHashesB;
        private readonly IPowerModule powerModule;

        public Zeni(IPowerModule powerModule, IEnumerable<string> inputTexts) 
        {
            this.bagOfText = inputTexts;
            this.powerModule = powerModule;

            this.bagOfHashesA = new HashSet<long>();
            this.bagOfHashesB = new HashSet<long>();

            this.Init();
        }

        public bool Has(long hashA, long hashB)
        {
            return bagOfHashesA.Contains(hashA) && bagOfHashesB.Contains(hashB);
        }

        private void Init()
        {
            foreach (string text in bagOfText)
            {
                var hashProvider = new IntervalHash(powerModule, text);
                for (int start = 0; start < text.Length; start++)
                    for (int end = start; end < text.Length; end++)
                    {
                        bagOfHashesA.Add(hashProvider.GetRangeHash(start, end, HashType.HashTypeA));
                        bagOfHashesB.Add(hashProvider.GetRangeHash(start, end, HashType.HashTypeB));
                    }
            }
        }
    }
}
