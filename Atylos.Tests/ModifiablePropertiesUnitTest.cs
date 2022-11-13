using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AtylosBase.PropertyModifire;

namespace Atylos.Tests
{
    public class ModifiablePropertiesUnitTest
    {
        private Modificators modificators;


        [SetUp]
        public void Setup()
        {
            modificators = new Modificators();
        }

        [Test]
        public void PriceChanged()
        {
            Debug.WriteLine(Modificators.PriceModificator.TargetName);

            var target = new ClassWithModifiableProperties(100, "Hi");

            Assert.That(target.Price, Is.EqualTo(600));
        }

        [Test]
        public void NamesChanged()
        {
            Debug.WriteLine(Modificators.PriceModificator.TargetName);

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
            Debug.WriteLine(Modificators.PriceModificator.TargetName);

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

            

        }
    }
}
