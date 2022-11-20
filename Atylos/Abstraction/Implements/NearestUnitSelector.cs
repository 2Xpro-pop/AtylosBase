using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atylos.Abstraction.Implements
{
    public class NearestUnitSelector : IUnitBattleSelector
    {
        public AtylosUnit Select(AtylosBattle battle, AtylosUnit atylosUnit)
        {
            var pos = atylosUnit.Position;

            var positions = atylosUnit.IsEnemy ? battle.Units : battle.UnitsEnemy;

            var distances = positions.Select(unit => (unit, Distance(unit.Position, pos)));

            var min = distances.Aggregate((c, d) => c.Item2 < d.Item2 ? c : d);

            return min.unit;
        }

        double Distance(BattlePosition a, BattlePosition b) => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}
