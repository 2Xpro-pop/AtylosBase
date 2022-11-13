using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AtylosBase.PropertyModifire
{
    public static class PropertiesExtensions
    {

        public static TPropety GetValue<TOwner, TPropety>(this TOwner owner, [CallerMemberName] string name = "")
        {
            var properties = PropertiesAndModificators.modifiableProperties;
            var property = (ModifiableProperty<TOwner, TPropety>)properties[owner][name];

            return property.ModifiedValue;
        }

        public static void SetValue<TOwner, TPropety>(this TOwner owner, TPropety value, [CallerMemberName] string name = "")
        {
            var properties = PropertiesAndModificators.modifiableProperties;
            var property = (ModifiableProperty<TOwner, TPropety>)properties[owner][name];

            property.Value = value;
        }

        public static IObservable<TPropety> WhenPropertyChanged<TOwner, TPropety>(this TOwner owner, Expression<Func<TOwner, TPropety>> expression)
        {
            if (!(expression.Body is MemberExpression member))
            {
                throw new ArgumentException("Need reference to property", nameof(expression));
            }

            var property = member.Member as PropertyInfo;
            var properties = PropertiesAndModificators.modifiableProperties;

            var modifiableProperty = (ModifiableProperty<TOwner, TPropety>)properties[owner][property.Name];

            return modifiableProperty.WhenAnyValue(x => x.ModifiedValue);
        }
    }
}
