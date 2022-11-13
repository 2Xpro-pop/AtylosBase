using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AtylosBase.PropertyModifire
{
    public class ModifiableProperty: ReactiveObject
    {
        public virtual string Name { get; }

        public ModifiableProperty(string name)
        {
            Name = name;
        }
    }
    public class ModifiableProperty<TOwner, TProperty>: ModifiableProperty
    {
        
        public TOwner Owner { get; }
        public TProperty Value 
        {
            get => _value;
            set
            {
                _value = value;
                _modifiedValue = value;

                var tValue = value;

                PropertiesExtensions.CreateModificatorIfNotExist<TOwner, TProperty>(Name);

                foreach(var modificator in PropertiesAndModificators.propertyModificators[TypeOf<TOwner>.Type][Name])
                {
                    if((bool)(modificator?.CanModify(Owner)))
                    {
                        tValue = (TProperty)modificator.Modify(Owner, tValue);
                    }
                }
                ModifiedValue = tValue;

                this.RaisePropertyChanged();
            }
            
        }
        private TProperty _value;


        public TProperty ModifiedValue 
        {
            get => _modifiedValue;
            private set => this.RaiseAndSetIfChanged(ref _modifiedValue, value);
        }
        private TProperty _modifiedValue;

        public ModifiableProperty(string name, TOwner owner) : base(name)
        {
            Owner = owner;
        }
    }
}
