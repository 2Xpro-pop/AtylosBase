﻿using Atylos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atylos
{
    public class AtylosBattle
    {
        
        public AtylosBattle(AtylosMatch atylosMatch, IReadOnlyList<AtylosUnit> units, IReadOnlyList<AtylosUnit> unitsEnemy)
        {
            AtylosMatch = atylosMatch;
            Units = units;
            UnitsEnemy = unitsEnemy;

            if (Units.Count > 6)
            {
                throw new ArgumentException("Maximum 6 units!", nameof(units));
            }

            if (UnitsEnemy.Count > 6)
            {
                throw new ArgumentException("Maximum 6 units!", nameof(unitsEnemy));
            }
            
        }

        public AtylosMatch AtylosMatch { get; }
        public IReadOnlyList<AtylosUnit> Units { get; }
        public IReadOnlyList<AtylosUnit> UnitsEnemy { get; }


        public AtylosUnit GetWithPosition(BattlePosition position, bool isEnemy)
        {
            if(isEnemy)
            {
                return UnitsEnemy.FirstOrDefault(unit => unit.Position == position);
            }
            else
            {
                return Units.FirstOrDefault(unit => unit.Position == position);
            }
        }
    }
}
