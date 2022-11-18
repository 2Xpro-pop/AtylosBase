using System;
using System.Collections.Generic;
using System.Text;

namespace Atylos
{
    public readonly struct BattlePosition
    {
        public int X { get; }
        public int Y { get; }

        public BattlePosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BattlePosition pos))
            {
                return false;
            }

            return pos.X == X && pos.Y == Y;
        }

        public static bool operator ==(BattlePosition a, BattlePosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BattlePosition a, BattlePosition b)
        {
            return !a.Equals(b);
        }
    }
}
