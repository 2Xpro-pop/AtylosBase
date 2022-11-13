using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Atylos.PropertyModifire
{
    public static class PropertiesExtensions
    {

        public static TPropety GetValue<TOwner, TPropety>(this TOwner owner, [CallerMemberName] string name = "")
        {
            CreatePropertyIfNotExist<TOwner, TPropety>(owner, name);

            var properties = PropertiesAndModificators.modifiableProperties;
            var property = (ModifiableProperty<TOwner, TPropety>)properties[owner][name];

            return property.ModifiedValue;
        }

        public static void SetValue<TOwner, TPropety>(this TOwner owner, TPropety value, [CallerMemberName] string name = "")
        {
            CreatePropertyIfNotExist<TOwner, TPropety>(owner, name);

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

        public static void CreatePropertyIfNotExist<TOwner, TPropety>(TOwner owner, [CallerMemberName] string name = "")
        {
            var properties = PropertiesAndModificators.modifiableProperties;

            if (!properties.ContainsKey(owner))
            {
                properties[owner] = new Dictionary<string, ModifiableProperty>();
            }

            if (!properties[owner].ContainsKey(name))
            {
                properties[owner][name] = new ModifiableProperty<TOwner, TPropety>(name, owner);
            }
        }

        public static void CreateModificatorIfNotExist<TOwner, TPropety>(string targetName)
        {
            var modificators = PropertiesAndModificators.propertyModificators;
            var type = TypeOf<TOwner>.Type;

            if (!modificators.ContainsKey(type))
            {
                modificators[type] = new Dictionary<string, List<PropertyModificator>>();
            }

            if (!modificators[type].ContainsKey(targetName))
            {
                modificators[type][targetName] = new List<PropertyModificator>();
            }
        }
    }
}
