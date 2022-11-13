using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.Abstraction
{
    public interface ILocalizedString
    {
        string this[string index] { get; }
    }
}
