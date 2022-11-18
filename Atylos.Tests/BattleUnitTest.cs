using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests
{
    public class BattleUnitTest
    {
        [Test]
        public void Test()
        {
            var dmg = new DamageInfo(this, DamageType.Pure, 123);

            Assert.That(dmg.DamageType, Is.EqualTo(DamageType.Pure));
        }

        enum DamageType { Pure, Magic }
    }
}
