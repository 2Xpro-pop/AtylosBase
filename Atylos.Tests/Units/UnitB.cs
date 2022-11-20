using Atylos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests.Units
{
    public class UnitB : UnitA
    {

        protected override void OnAtacking(ref DamageInfo damageInfo, AtylosUnit enemy)
        {

        }

        protected override void OnTakingDamage(ref DamageInfo damageInfo)
        {
            if(damageInfo.Sender is AtylosUnit unit)
            {
                unit.TakeDamage(
                    new DamageInfo(new ReturnedDamage(this), null, damageInfo.Damage / 2)
                );
            }
        }
    }
}
