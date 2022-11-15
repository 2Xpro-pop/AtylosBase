using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atylos.ModifiableProperty
{
    internal static class PropertiesAndModificators
    {
        internal readonly static Dictionary<object, Dictionary<string, ModifiableProperty>> modifiableProperties =
            new Dictionary<object, Dictionary<string, ModifiableProperty>>();

        internal readonly static Dictionary<Type, Dictionary<string, List<PropertyModificator>>> propertyModificators =
            new Dictionary<Type, Dictionary<string, List<PropertyModificator>>>();

        internal static void UpdateProperties<TOwner>(string name)
        {
            var owners = modifiableProperties.Where(kv => kv.Key is TOwner);

            foreach (var owner in owners)
            {
                foreach (var prop in owner.Value)
                {
                    if (prop.Key == name)
                    {
                        prop.Value.UpdateValue();
                    }
                }
            }
        }
    }
}
