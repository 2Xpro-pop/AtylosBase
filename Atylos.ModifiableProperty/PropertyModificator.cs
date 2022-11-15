using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Atylos.ModifiableProperty
{
    public class PropertyModificator : IDisposable, IComparable<PropertyModificator>
    {
        public event Action<PropertyModificator> Disposed;

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

        public void Dispose()
        {
            var modificators = PropertiesAndModificators.propertyModificators[TargetType][TargetName];
            modificators.Remove(this);

            Disposing(this);

            GC.SuppressFinalize(this);
        }

        protected void Disposing(PropertyModificator propertyModificator) => Disposed?.Invoke(propertyModificator);

        public int CompareTo(PropertyModificator other)
        {
            return Order.CompareTo(other.Order);
        }

        /// <summary>
        /// Create modificator
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="target"></param>
        /// <param name="modificator"></param>
        /// <param name="predicate"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static PropertyModificator<TTarget, TProperty> CreateStaticModificator<TTarget, TProperty>(
            Expression<Func<TTarget, TProperty>> target,
            Func<TTarget, TProperty, TProperty> modificator,
            Func<TTarget, bool> predicate = null,
            float order = float.MaxValue)
        {
            predicate = predicate ?? (x => true);

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

            PropertiesExtensions.CreateModificatorIfNotExist<TTarget, TProperty>(property.Name);

            var modificators = PropertiesAndModificators.propertyModificators[TypeOf<TTarget>.Type][property.Name];

            modificators.AddSorted(propertyModificator);

            propertyModificator.Disposed += propMod =>
            {
                PropertiesAndModificators.UpdateProperties<TTarget>(property.Name);
            };

            return propertyModificator;
        }

        public static PropertyModificator<TOwner, TTarget, TProperty> CreateModificator<TOwner, TTarget, TProperty>(
            Expression<Func<TTarget, TProperty>> target,
            float order = float.MaxValue)
        {

            if (!(target.Body is MemberExpression member))
            {
                throw new ArgumentException("Need reference to property", nameof(target));
            }

            var property = member.Member as PropertyInfo;

            PropertiesExtensions.CreateModificatorIfNotExist<TTarget, TProperty>(property.Name);

            var propertyModificator = new PropertyModificator<TOwner, TTarget, TProperty>(property.Name, order);

            var modificators = PropertiesAndModificators.propertyModificators[TypeOf<TTarget>.Type][property.Name];

            modificators.AddSorted(propertyModificator);

            return propertyModificator;
        }
    }
}
