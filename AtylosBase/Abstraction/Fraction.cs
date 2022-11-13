using System;
using System.Collections.Generic;
using System.Text;

namespace AtylosBase.Abstraction
{
    public abstract class Fraction
    {
        public abstract string Name { get; protected set; }
        public abstract string Description { get; protected set; }

        internal void AfterBuild(AtylosMatch atylos)
        {
            var localized = atylos.Services.GetService<ILocalizedString>();

            Name = localized[Name];
        }
    }
}
