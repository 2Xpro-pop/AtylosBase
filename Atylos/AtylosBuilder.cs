using Atylos.Abstraction;
using Atylos.Abstraction.Implements;
using Atylos.ScopableServiceProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos
{
    public class AtylosBuilder
    {
        public ScopableServiceProviderBuilder Services { get; } = new ScopableServiceProviderBuilder();
        private List<Fraction> Fractions { get; } = new List<Fraction>();

        public AtylosBuilder() 
        {
            Services.AddBattle<IUnitBattleSelector, NearestUnitSelector>();
        }

        public AtylosMatch Build()
        {
            var match = new AtylosMatch(Services.Build(), Fractions.ToArray());

            return match;
        }
    }
}
