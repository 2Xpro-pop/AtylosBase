using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Atylos.ScopableServiceProvider
{
    /// <summary>
    /// Why poor? Because compared to Microsoft.Extensions.DependencyInjection.Abstractions it is poor in features.
    /// </summary>
    public class PoorScopableServiceProvider : IServiceProvider
    {
        private readonly IReadOnlyDictionary<Type, ServiceDescriptor> _serviceDescriptors;
        private readonly Dictionary<Enum, List<ServiceDescriptor>> _scopeServices = new Dictionary<Enum, List<ServiceDescriptor>>();

        public PoorScopableServiceProvider(IReadOnlyDictionary<Type, ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType)
        {
            if(!_serviceDescriptors.ContainsKey(serviceType))
            {
                return null;
            }

            var descriptor = _serviceDescriptors[serviceType];

            if(descriptor.Scope == null)
            {
                return ActivatorUtilities.CreateInstance(this, descriptor.InstanceType);
            }

            return descriptor.Instance;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IDisposable ActivateScope(Enum scope)
        {
            CreateIfNull(_scopeServices, scope);

            foreach(var descriptor in _serviceDescriptors.Values.Where(d => d.Scope != null && d.Scope.Equals(scope)))
            {
                descriptor.Instance = ActivatorUtilities.CreateInstance(this, descriptor.InstanceType);
                Console.WriteLine("");
                _scopeServices[scope].Add(descriptor);
            }

            return Disposable.Create(() =>
            {
                foreach(var service in _scopeServices[scope])
                {
                    if(service.Instance is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    service.Instance = null;
                }

                _scopeServices[scope].Clear();
            });
        }

        static void CreateIfNull(Dictionary<Enum, List<ServiceDescriptor>> self, Enum key)
        {
            if (!self.ContainsKey(key)) self[key] = new List<ServiceDescriptor>();
        }
    }
}
