using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests.Units
{
    public readonly struct ReturnedDamage
    {
        public ReturnedDamage(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
    }
}
