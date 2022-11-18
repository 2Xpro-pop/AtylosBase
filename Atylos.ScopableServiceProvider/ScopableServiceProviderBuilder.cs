using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ScopableServiceProvider
{
    public class ScopableServiceProviderBuilder: IDictionary<Type, ServiceDescriptor>
    {
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();

        public void AddScoped<T, U>(Enum scope)
        {
            _serviceDescriptors[typeof(T)] = new ServiceDescriptor()
            {
                Scope = scope,
                InstanceType = typeof(U)
            };
        }

        public void AddScoped<T>(Enum scope) => AddScoped<T, T>(scope);

        public void AddSingleton<T, U>() where U : T => AddScoped<T, U>(BasicScope.Singletone);

        public void AddSingleton<T>() => AddSingleton<T, T>();

        public void AddTransient<T, U>() where U : T => AddScoped<T, U>(null);

        public void AddTransient<T>() => AddTransient<T, T>();

        public ScopableServiceProvider Build()
        {
            var serviceProvider = new ScopableServiceProvider(_serviceDescriptors);
            serviceProvider.ActivateScope(BasicScope.Singletone);

            return serviceProvider;
        }

        public enum BasicScope
        {
            Singletone
        }

        #region IDictionary implenetation
        public ServiceDescriptor this[Type key] { get => ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors)[key]; set => ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors)[key] = value; }

        public ICollection<Type> Keys => ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).Keys;

        public ICollection<ServiceDescriptor> Values => ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).Values;

        public int Count => ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).IsReadOnly;

        public void Add(Type key, ServiceDescriptor value)
        {
            ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).Add(key, value);
        }

        public void Add(KeyValuePair<Type, ServiceDescriptor> item)
        {
            ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).Clear();
        }

        public bool Contains(KeyValuePair<Type, ServiceDescriptor> item)
        {
            return ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).Contains(item);
        }

        public bool ContainsKey(Type key)
        {
            return ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Type, ServiceDescriptor>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<Type, ServiceDescriptor>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).GetEnumerator();
        }

        public bool Remove(Type key)
        {
            return ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).Remove(key);
        }

        public bool Remove(KeyValuePair<Type, ServiceDescriptor> item)
        {
            return ((ICollection<KeyValuePair<Type, ServiceDescriptor>>)_serviceDescriptors).Remove(item);
        }

        public bool TryGetValue(Type key, out ServiceDescriptor value)
        {
            return ((IDictionary<Type, ServiceDescriptor>)_serviceDescriptors).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_serviceDescriptors).GetEnumerator();
        }
        #endregion
        
    }
}
