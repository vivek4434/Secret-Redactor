namespace Secret.Redactor.TwoPointer
{
    using Secret.Redactor.API;
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
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            List<(int Start, int End)> intervals = new List<(int, int)>();
            int n = input.Length;
            int i = 0, j = 0;
            long hash31 = 0, hash257 = 0;

            while (j < n)
            {
                if (!this.hasher.IsSecretCharacter(input[j]))
                {
                    hash31 = 0;
                    hash257 = 0;
                    j++;
                    i = j;
                    continue;
                }

                // Update the hash values for the current window
                (long tmphash31, long tmphash257) = hasher.UpdateHashes(input[j], hash31, hash257);

                if (hasher.Match(tmphash31, tmphash257))
                {
                    // intervals format[i, j)
                    intervals.Add((i, j + 1));
                    hash257 = tmphash257;
                    hash31 = tmphash31;
                    j++;
                }
                else
                {
                    // Incrementally update the hash values for the new window [i+1, j]
                    hash31 = hasher.RemoveHash(input[i], hash31, Constants.PrimaryPrime);
                    hash257 = hasher.RemoveHash(input[i], hash257, Constants.SecondaryPrime);
                    i++;
                }

                // Ensure i <= j
                if (i > j)
                {
                    j = i;
                }
            }

            intervals = MergeIntervals(intervals);

            return input.MaskString(intervals);
        }

        /// <summary>
        /// Merges overlapping intervals.
        /// </summary>
        /// <param name="intervals">The list of intervals to merge.</param>
        /// <returns>The merged list of intervals.</returns>
        private List<(int, int)> MergeIntervals(List<(int Start, int End)> intervals)
        {
            if (intervals.Count == 0) return intervals;

            List<(int, int)> merged = new List<(int, int)>();
            (int Start, int End) current = intervals[0];

            foreach (var interval in intervals.Skip(1))
            {
                if (interval.Start < current.End)
                {
                    current.End = Math.Max(current.End, interval.End);
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
