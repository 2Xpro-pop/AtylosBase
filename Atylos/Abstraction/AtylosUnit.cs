using Atylos.ModifiableProperty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos.Abstraction
{
    public abstract class AtylosUnit
    {
        public BattlePosition Position { get; set; }

        public bool IsEnemy { get; set; }

        public int Damage
        {
            get => this.GetValue<AtylosUnit, int>();
            set => this.SetValue(value);
        }
        public Enum DamageType
        {
            get => this.GetValue<AtylosUnit, Enum>();
            set => this.SetValue(value);
        }

        public int Hp
        {
            get => this.GetValue<AtylosUnit, int>();
            set => this.SetValue(value);
        }

        public AtylosBattle Battle { get; set; }



        protected abstract void OnTakingDamage(ref DamageInfo damageInfo);
        protected abstract void OnAtacking(ref DamageInfo damageInfo, AtylosUnit enemy);

        public virtual void Atack()
        {
            var selector = Battle.AtylosMatch.Services.GetService<IUnitBattleSelector>();

            var enemy = selector.Select(Battle, this);

            var dmgInfo = new DamageInfo(this, DamageType, Damage);

            OnAtacking(ref dmgInfo, enemy);

            enemy.TakeDamage(dmgInfo);
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            OnTakingDamage(ref damageInfo);

            Hp -= damageInfo.Damage;
        }
    }
}
