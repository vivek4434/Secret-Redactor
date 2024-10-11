namespace Secret.Redactor.Hash.Generator
{
    using Secret.Redactor.Hash.Generator.Enums;
    using Secret.Redactor.Hash.Generator.Interfaces;

    /// <summary>
    /// Responsible for finding possible substring matches.
    /// </summary>
    public class MatchFinder
    {
        private readonly Zeni hashService;
        private readonly IPowerModule powerModule;
        private readonly IIntervalHash hashGenerator;

        public MatchFinder(Zeni hashService,
            IPowerModule powerModule,
            string text)
        {
            this.hashService = hashService;
            this.powerModule = powerModule;

            this.hashGenerator = new IntervalHash(powerModule, text);
        }

        public bool IsPresent(int start, int end)
        {
            long hashA = this.hashGenerator.GetRangeHash(start, end, HashType.HashTypeA);
            long hashB = this.hashGenerator.GetRangeHash(start, end, HashType.HashTypeB);
            return this.hashService.Has(hashA, hashB);
        }
    }
}
