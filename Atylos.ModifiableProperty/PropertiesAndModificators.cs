using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ModifiableProperty
{
    internal static class PropertiesAndModificators
    {
        internal readonly static Dictionary<object, Dictionary<string, ModifiableProperty>> modifiableProperties =
            new Dictionary<object, Dictionary<string, ModifiableProperty>>();

        internal readonly static Dictionary<Type, Dictionary<string, List<PropertyModificator>>> propertyModificators =
            new Dictionary<Type, Dictionary<string, List<PropertyModificator>>>();
    }
}
