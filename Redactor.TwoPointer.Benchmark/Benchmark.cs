namespace Redactor.TwoPointer.Benchmark
{
    using BenchmarkDotNet.Running;

    public class Benchmarking
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessLineByLine_2>();
            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessLineByLine_3>();
            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessLineByLine_4>();

            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessAsContent_1>();
            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessAsContent_2>();
            //BenchmarkRunner.Run<BM_500Secrets_500Lines5KChars_ProcessAsContent_3>();

            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessLineByLine_1>();
            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessLineByLine_2>();
            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessLineByLine_3>();
            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessLineByLine_4>();

            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessAsContent_1>();
            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessAsContent_2>();
            //BenchmarkRunner.Run<BM_500Secrets_8KLines20MChars_ProcessAsContent_3>();

            BenchmarkRunner.Run<TestCase_500Secrets_500Lines5KChars_ProcessAsContent_1>();
            BenchmarkRunner.Run<TestCase_500Secrets_500Lines5KChars_ProcessLineByLine_1>();
        }
    }
}
