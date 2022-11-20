using Atylos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests.Units
{
    public class UnitA : AtylosUnit
    {
        public UnitA() 
        {
            Damage = 4;
            Hp = 15;
        }

        protected override void OnAtacking(ref DamageInfo damageInfo, AtylosUnit enemy)
        {

        }

        protected override void OnTakingDamage(ref DamageInfo damageInfo)
        {
            
        }
    }
}
