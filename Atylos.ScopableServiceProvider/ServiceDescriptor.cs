using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.ScopableServiceProvider
{
    public class ServiceDescriptor
    {
        public virtual object Instance { get; set; }
        public virtual Enum Scope { get; set; }
        public virtual Type InstanceType { get; set; }
    }
}
