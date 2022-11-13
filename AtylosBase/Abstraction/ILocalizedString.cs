using System;
using System.Collections.Generic;
using System.Text;

namespace AtylosBase.Abstraction
{
    public interface ILocalizedString
    {
        string this[string index] { get; }
    }
}
