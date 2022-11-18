using Atylos.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos
{
    public readonly struct AtylosUnitInfo
    {
        public AtylosUnitInfo(string name, string description, int damage)
        {
            Name = name;
            Description = description;
            Damage = damage;
            
        }

        public string Name { get; }
        public string Description { get; }
        public int Damage { get; }
    }
}
