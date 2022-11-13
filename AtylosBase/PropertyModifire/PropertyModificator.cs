﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AtylosBase.PropertyModifire
{
    public class PropertyModificator : IDisposable, IComparable<PropertyModificator>
    {
        private readonly Func<object, bool> _predicate;
        private readonly Func<object, object, object> _modificator;

        public PropertyModificator(string targetName, Func<object, object, object> modificator, Func<object, bool> predicate, float order)
        {
            _predicate = predicate;
            _modificator = modificator;
            Order = order;
            TargetName = targetName;
        }
        public virtual bool CanModify(object target) => _predicate.Invoke(target);
        public virtual object Modify(object target, object value) => _modificator.Invoke(target, value);

        public virtual Type TargetType { get; }
        public virtual string TargetName { get; }
        public virtual float Order { get; }

        public void Dispose() => GC.SuppressFinalize(this);


        public static PropertyModificator<TTarget, TProperty> CreateStaticModificator<TTarget, TProperty>(
            Expression<Func<TTarget, TProperty>> target,
            Func<TTarget, TProperty, TProperty> modificator, 
            Func<TTarget, bool> predicate = null, 
            float order = float.MaxValue)
        {
            predicate = x => true;

            if (!(target.Body is MemberExpression member))
            {
                throw new ArgumentException("Need reference to property", nameof(target));
            }

            var property = member.Member as PropertyInfo;


            var propertyModificator = new PropertyModificator<TTarget, TProperty>(
                    property.Name,
                    modificator, 
                    predicate,
                    order
                );

            var modificators = PropertiesAndModificators.propertyModificators[TypeOf<TTarget>.Type][property.Name];

            modificators.AddSorted(propertyModificator);

            return propertyModificator;
        }

        public int CompareTo(PropertyModificator other)
        {
            return Order.CompareTo(other.Order);
        }
    }
    public class PropertyModificator<TTarget, TProperty>: PropertyModificator
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