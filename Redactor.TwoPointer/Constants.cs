using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secret.Redactor.TwoPointer
{
    /// <summary>
    /// Placeholder for Constants.
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// Modulo.
        /// </summary>
        internal const long Mod = 1000000007;

        /// <summary>
        /// Mask to hide secret in input string.
        /// </summary>
        internal const string Mask = "$PASS";

        /// <summary>
        /// Primary prime to calculate rolling hash.
        /// </summary>
        internal const int PrimaryPrime = 31;

        /// <summary>
        /// Secondary prime to calculate rolling hash.
        /// </summary>
        internal const int SecondaryPrime = 257;
    }
}
