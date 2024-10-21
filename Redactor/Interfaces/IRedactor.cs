using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secret.Redactor.Hash.Generator.Interfaces
{
    /// <summary>
    /// Exposes API for secret redaction.
    /// </summary>
    public interface IRedactor
    {
        /// <summary>
        /// Redacts sensitive information from the input string.
        /// </summary>
        /// <param name="input">The input string to redact.</param>
        /// <returns>The redacted string.</returns>
        string Redact(string input);
    }
}
