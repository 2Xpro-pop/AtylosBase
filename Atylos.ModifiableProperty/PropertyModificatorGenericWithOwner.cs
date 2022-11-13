using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Atylos.ModifiableProperty
{
    public class PropertyModificator<TOwner, TTarget, TProperty> : PropertyModificator<TTarget, TProperty>
    {
        public PropertyModificator(
            string targetName,
            Func<TTarget, TProperty, TProperty> modificator,
            Func<TTarget, bool> predicate,
            float order)
            : base(targetName, modificator, predicate, order) { }


    }
}
