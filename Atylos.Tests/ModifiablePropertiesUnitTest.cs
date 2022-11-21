using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Atylos.ModifiableProperty;

namespace Atylos.Tests
{
    public class ModifiablePropertiesUnitTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PriceChanged()
        {
            Console.WriteLine(Modificators.PriceModificator.TargetName);

            var target = new ClassWithModifiableProperties(100, "Hi");

            Assert.That(target.Price, Is.EqualTo(600));
        }

        [Test]
        public void NamesChanged()
        {
            Console.WriteLine(Modificators.PriceModificator.TargetName);

            var target = new ClassWithModifiableProperties(100, "Hi");
            var target2 = new ClassWithModifiableProperties(100, "uppercase");

            Assert.Multiple(() =>
            {
                Assert.That(target.Name, Is.EqualTo("Ha"));
                Assert.That(target2.Name, Is.EqualTo("UaPaRaAaE"));
            });

            
        }

        [Test]
        public void PropertiesObserved()
        {
            Console.WriteLine(Modificators.PriceModificator.TargetName);

            var target = new ClassWithModifiableProperties(100, "uppercase")
            {
                Price = 90,
                Name = "p0p"
            };

            var dispose1 = target.WhenPropertyChanged(x => x.Name).Subscribe(name =>
            {
                Assert.That(name, Is.EqualTo("pap"));
                    
            });

            var dispose2 = target.WhenPropertyChanged(x => x.Price).Subscribe(price =>
            {
                Assert.That(price, Is.EqualTo(90*6));
            });


            target.Name = "sada";
        }

        [Test]
        public void NonStaticModificator()
        {
            var target = new ClassWithModifiableProperties(100, "uppercase");

            using(var modificators = new Modificators())
            {
                using(new Modificators())
                {
                    Assert.That(target.Incomming, Is.EqualTo(60));
                }
                Assert.That(target.Incomming, Is.EqualTo(20));
            }
            Assert.That(target.Incomming, Is.EqualTo(0));
        }
    }
}
