using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ModifiableProperty
{
    internal static class TypeOf<T>
    {
        public static readonly Type Type = typeof(T);
    }
}
