using Atylos.Abstraction;
using Atylos.ScopableServiceProvider;
using Atylos.Tests.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Atylos.Tests
{
    public class BattleUnitTest
    {
        private AtylosMatch match;

        [SetUp]
        public void Setup()
        {
            var builder = new AtylosBuilder();

            match = builder.Build();
        }

        [Test]
        public void Battle()
        {
            var units = new AtylosUnit[] { new UnitB { Position = new BattlePosition(-1, 0) } }; 
            var enemies = new AtylosUnit[] { new UnitA { Position = new BattlePosition(1, 0) }, new UnitA() { Position = new BattlePosition(2,0) } }; 

            var battle = match.StartBattle(units, enemies);

            units[0].Atack();

            enemies[0].Atack();
            enemies[1].Atack();

            Assert.Multiple(() =>
            {
                Assert.That(units[0].Hp, Is.EqualTo(7));
                Assert.That(enemies[0].Hp, Is.EqualTo(9));
                Assert.That(enemies[1].Hp, Is.EqualTo(13));
            });
        }

        [Test]
        public void Test()
        {
            var dmg = new DamageInfo(this, DamageType.Pure, 123);

            Assert.That(dmg.DamageType, Is.EqualTo(DamageType.Pure));
        }

        enum DamageType { Pure, Magic }
    }
}
