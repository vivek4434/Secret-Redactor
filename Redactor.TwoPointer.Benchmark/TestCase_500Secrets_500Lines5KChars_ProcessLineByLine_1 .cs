using BenchmarkDotNet.Attributes;
using Redactor.TwoPointer.Benchmark.Cases;
using Secret.Redactor.Hash.Generator;

namespace Redactor.TwoPointer.Benchmark
{
    public class TestCase_500Secrets_500Lines5KChars_ProcessLineByLine_1 : Case_500Secrets_500Lines5KChars_ProcessLineByLine_Base
    {
        [Benchmark(Description = "Scrub (unique secrets:500), Input: (5K chars, 100 lines), process line by line")]
        public void Scrub()
        {
            var hasher = new Hasher();
            hasher.GenerateHashes(magicSecrets);
            var redactor = new Secret.Redactor.TwoPointer.Redactor(hasher);
            for (int i = 0; i < content_lines_1.Length; i++)
            {
                content_lines_1[i] = redactor.Redact(content_lines_1[i]);
            }
        }
    }
}
