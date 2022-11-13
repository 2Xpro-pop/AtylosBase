using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AtylosBase
{
    internal class SimpleServiceProvider : IServiceProvider, IEnumerable<object>
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public object GetService(Type serviceType)
        {
            return _services[serviceType];
        }

        public void AddService<T,U>() where U: T, new()
        {
            _services.Add(TypeOf<T>.Type, new U());
        }

        public void AddService<T>(T service) 
        {
            _services.Add(TypeOf<T>.Type, service);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _services.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _services.Values.GetEnumerator();
        }
    }
}
