namespace Redactor.TwoPointer.Benchmark.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;

    public class Case_500Secrets_500Lines5KChars_ProcessAsContent_Base
    {
        protected readonly List<string> magicSecrets = new List<string>();
        protected readonly IImmutableList<string> immutableMagicSecrets;
        protected readonly string content_1;
        protected string secret = @"ThisIsMySecret&&$$%%FF|.0";
        public Case_500Secrets_500Lines5KChars_ProcessAsContent_Base()
        {
            content_1 = File.ReadAllText("input.txt");

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()}[]}";

            Enumerable.Range(0, 500).ToList().ForEach(x =>
            {
                var rs = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
                magicSecrets.Add(rs);
            });

            magicSecrets.Add(secret);

            immutableMagicSecrets = magicSecrets.ToImmutableList();
        }
    }
}
