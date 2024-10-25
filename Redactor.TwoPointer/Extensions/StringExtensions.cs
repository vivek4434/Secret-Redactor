namespace Secret.Redactor.TwoPointer.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    public static class StringExtensions
    {
        private const byte minIntervalLength = 3;

        public static string MaskString(this string input, List<(int Start, int End)> intervals)
        {
            if (string.IsNullOrEmpty(input) || intervals == null || intervals.Count == 0)
            {
                return input;
            }

            StringBuilder sb = new StringBuilder();
            int currIntervalPtr = 0;
            int index = 0;

            while (index < input.Length)
            {
                if (currIntervalPtr < intervals.Count && index == intervals[currIntervalPtr].Start)
                {
                    if (intervals[currIntervalPtr].End - intervals[currIntervalPtr].Start >= minIntervalLength)
                    {
                        // as [start, end - 1] will be replaced by $PASS.
                        sb.Append(Constants.Mask);
                        currIntervalPtr++;
                    }
                    else
                    {
                        // For smaller interval, retaining all characters
                        while (index < intervals[currIntervalPtr].End)
                        {
                            sb.Append(input[index]);
                            index++;
                        }
                    }

                    // interval: [start, end) => advancing to index = end
                    index = intervals[currIntervalPtr].End;
                }
                else
                {
                    sb.Append(input[index]);
                    index++;
                }
            }

            return sb.ToString();
        }
    }
}
