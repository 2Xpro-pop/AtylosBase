﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AtylosBase.PropertyModifire
{
    internal static class PropertiesAndModificators
    {
        internal readonly static Dictionary<object, Dictionary<string, ModifiableProperty>> modifiableProperties = 
            new Dictionary<object, Dictionary<string,ModifiableProperty>>();

        internal readonly static Dictionary<Type, Dictionary<string, List<PropertyModificator>>> propertyModificators =
            new Dictionary<Type, Dictionary<string, List<PropertyModificator>>>();

    }
}