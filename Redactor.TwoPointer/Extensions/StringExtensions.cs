namespace Secret.Redactor.TwoPointer.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    public static class StringExtensions
    {
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
                    while (index < input.Length && index <= intervals[currIntervalPtr].End)
                    {
                        index++;
                    }

                    sb.Append(Constants.Mask);
                    currIntervalPtr++;
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
