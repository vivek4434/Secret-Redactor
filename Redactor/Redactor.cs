namespace Secret.Redactor.TwoPointer
{
    using Secret.Redactor.Hash.Generator.Interfaces;
    using Secret.Redactor.TwoPointer.Extensions;
    using Secret.Redactor.TwoPointer.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Redactor class for redacting sensitive information using the two-pointer technique.
    /// </summary>
    public class Redactor : IRedactor
    {
        private readonly IHasher hasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Redactor"/> class.
        /// </summary>
        /// <param name="hasher">The hasher used for generating and matching hash values.</param>
        public Redactor(IHasher hasher)
        {
            this.hasher = hasher;
        }

        /// <inheritdoc/>
        public string Redact(string input)
        {
            List<(int, int)> intervals = new List<(int, int)>();
            int n = input.Length;
            int i = 0, j = 0;
            long hash31 = 0, hash257 = 0;

            while (j < n)
            {
                if (!this.hasher.IsSecretCharacter(input[j]))
                {
                    if (i != j)
                    {
                        hash31 = 0;
                        hash257 = 0;
                        i = j + 1;
                    }
                    j++;
                    continue;
                }

                // Update the hash values for the current window
                (hash31, hash257) = hasher.UpdateHashes(input[j], hash31, hash257);

                if (hasher.Match(hash31, hash257))
                {
                    intervals.Add((i, j + 1));
                    i = j + 1;
                    hash31 = 0;
                    hash257 = 0;
                }

                j++;
            }

            intervals = MergeIntervals(intervals);

            return input.MaskString(intervals);
        }

        /// <summary>
        /// Merges overlapping intervals.
        /// </summary>
        /// <param name="intervals">The list of intervals to merge.</param>
        /// <returns>The merged list of intervals.</returns>
        private List<(int, int)> MergeIntervals(List<(int, int)> intervals)
        {
            if (intervals.Count == 0) return intervals;

            List<(int, int)> merged = new List<(int, int)>();
            (int, int) current = intervals[0];

            foreach (var interval in intervals.Skip(1))
            {
                if (interval.Item1 <= current.Item2)
                {
                    current.Item2 = Math.Max(current.Item2, interval.Item2);
                }
                else
                {
                    merged.Add(current);
                    current = interval;
                }
            }

            merged.Add(current);
            return merged;
        }
    }
}
