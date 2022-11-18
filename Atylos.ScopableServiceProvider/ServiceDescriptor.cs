using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ScopableServiceProvider
{
    public class ServiceDescriptor
    {
        public object Instance { get; set; }
        public Enum Scope { get; set; }
        public Type InstanceType { get; set; }
    }
}
