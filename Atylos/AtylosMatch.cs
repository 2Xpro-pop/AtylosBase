using Atylos.Abstraction;
using Atylos.ScopableServiceProvider;
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


        public AtylosBattle StartBattle(IReadOnlyList<AtylosUnit> units, IReadOnlyList<AtylosUnit> enemies)
        {
            var services = (IScopableServiceProvider)Services;

            var battle = new AtylosBattle(this, units, enemies);

            var scope = services.ActivateScope(AtylosScopes.BattleScope);

            foreach(var unit in units)
            {
                unit.Battle = battle;
                unit.IsEnemy = false;
            }

            foreach (var enemy in enemies)
            {
                enemy.Battle = battle;
                enemy.IsEnemy = true;
            }

            battle.BattleEnd.Subscribe(result =>
            {
                scope.Dispose();
            });

            return battle;
        }
    }
}
