using AtylosBase.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtylosBase
{
    public class AtylosBuilder
    {
        private readonly SimpleServiceProvider _serviceProvider = new SimpleServiceProvider();
        private List<Fraction> Fractions { get; } = new List<Fraction>();

        public AtylosBuilder() { }

        public void AddService<T, U>() where U : T, new()
        {
            _serviceProvider.AddService<T, U>();
        }

        public void AddService<T>(T service)
        {
            _serviceProvider.AddService(service);
        }

        public AtylosMatch Build()
        {
            var match = new AtylosMatch(_serviceProvider, Fractions.ToArray());

            foreach(var service in _serviceProvider)
            {
                if(service is IBuildingEvent building)
                {
                    building.OnBuilding(match);
                }
            }

            return match;
        }
    }
}
