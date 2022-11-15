using Atylos.ModifiableProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests
{
    internal class Modificators: IDisposable
    {
        public static readonly PropertyModificator<ClassWithModifiableProperties, int> PriceModificator =
            PropertyModificator.CreateStaticModificator(
                (ClassWithModifiableProperties x) => x.Price,
                (target, property) => property * 6
            );

        public static readonly PropertyModificator<ClassWithModifiableProperties, string> NameUpperModificator =
            PropertyModificator.CreateStaticModificator(
                (ClassWithModifiableProperties x) => x.Name,
                (target, property) => property.ToUpper(),
                predicate => predicate.Name?.Length > 5
            );
        public static readonly PropertyModificator<ClassWithModifiableProperties, string> NameSeccondALetterModificator =
            PropertyModificator.CreateStaticModificator(
                (ClassWithModifiableProperties x) => x.Name,
                (target, property) =>
                {
                    return string.Join("", property.Select((c, index) =>
                    {
                        return (index+1) % 2 == 0 ? 'a' : c; 
                    }));
                }
            );

        public static readonly PropertyModificator<Modificators, ClassWithModifiableProperties, double> IncommingModificator =
            PropertyModificator.CreateModificator<Modificators, ClassWithModifiableProperties, double>(
                x => x.Incomming
            );

        private IDisposable[] _disposables;

        public Modificators()
        {
            _disposables = new IDisposable[]
            {
                IncommingModificator.UntilDispose(this, x => x * 2 + 20)
            };
        }

        public void Dispose()
        {
            for(var i = 0; i < _disposables.Length; i++)
            {
                _disposables[i].Dispose();
            }
        }
    }
}
