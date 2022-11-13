using Atylos.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos
{
    public class AtylosMatch
    {
        
        public IServiceProvider Services { get; }
        public Fraction[] Fractions { get; }

        public AtylosMatch(IServiceProvider serviceProvider, Fraction[] fractions)
        {
            Services = serviceProvider;
            Fractions = fractions;
        }


    }
}
