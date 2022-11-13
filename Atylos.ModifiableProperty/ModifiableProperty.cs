using ReactiveUI;
using System;

namespace Atylos.ModifiableProperty
{
    public class ModifiableProperty : ReactiveObject
    {
        public virtual string Name { get; }

        public ModifiableProperty(string name)
        {
            Name = name;
        }
    }
}
