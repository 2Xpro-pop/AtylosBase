using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos
{
    public class DamageInfo
    {
        public DamageInfo(object sender, Enum damageType, int damage)
        {
            Sender = sender;
            DamageType = damageType;
            Damage = damage;
        }

        public object Sender { get; set; }
        public Enum DamageType { get; set; }
        public int Damage { get; set; }
    }
}
