using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Atylos.ModifiableProperty
{
    public class PropertyModificator<TTarget, TProperty> : PropertyModificator
    {

        public PropertyModificator(string targetName, Func<TTarget, TProperty, TProperty> modificator, Func<TTarget, bool> predicate, float order) : base(
            targetName,
            (a, b) => modificator((TTarget)a, (TProperty)b),
            x => predicate((TTarget)x),
            order)
        { }

        public override Type TargetType { get; } = TypeOf<TTarget>.Type;

        public virtual bool CanModify(TTarget target) => base.CanModify(target);
        public virtual TProperty Modify(TTarget target, TProperty value) => (TProperty)base.Modify(target, value);
    }
}
