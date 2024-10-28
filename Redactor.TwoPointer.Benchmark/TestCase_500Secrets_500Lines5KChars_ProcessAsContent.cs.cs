namespace Redactor.TwoPointer.Benchmark
{
    using BenchmarkDotNet.Attributes;
    using Redactor.TwoPointer.Benchmark.Cases;
    using Secret.Redactor.Hash.Generator;

    [MemoryDiagnoser]
    public class TestCase_500Secrets_500Lines5KChars_ProcessAsContent_1 : Case_500Secrets_500Lines5KChars_ProcessAsContent_Base
    {
        [Benchmark(Description = "Scrub (unique secrets:500), Input: (5K chars, 100 lines), process as content")]
        public void Scrub()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(magicSecrets);
            var redactor = new Secret.Redactor.TwoPointer.Redactor(hasher);
            redactor.Redact(content_1);
        }
    }
}
