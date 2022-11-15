using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atylos.ModifiableProperty;
namespace Atylos.Tests
{
    public class ClassWithModifiableProperties
    {
        public int Price
        {
            get => this.GetValue<ClassWithModifiableProperties, int>();
            set => this.SetValue(value);
        }

        public string Name 
        {
            get => this.GetValue<ClassWithModifiableProperties, string>();
            set => this.SetValue(value);
        } 

        public double Incomming
        {
            get => this.GetValue<ClassWithModifiableProperties, double>();
            set => this.SetValue(value);
        }

        public ClassWithModifiableProperties(int price, string name)
        {
            Price = price;
            Name = name;
            Incomming = 0;
        }
    }
}
