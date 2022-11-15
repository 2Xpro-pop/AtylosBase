using ReactiveUI;
using System;

namespace Atylos.ModifiableProperty
{
    public abstract class ModifiableProperty : ReactiveObject
    {
        public virtual string Name { get; }

        public abstract void UpdateValue();

        public ModifiableProperty(string name)
        {
            Name = name;
        }
    }
}
