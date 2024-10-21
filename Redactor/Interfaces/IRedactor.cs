using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secret.Redactor.Hash.Generator.Interfaces
{
    public interface IRedactor
    {
        string Redact(string input);
    }
}
