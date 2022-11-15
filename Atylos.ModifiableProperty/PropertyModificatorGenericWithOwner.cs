using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Atylos.ModifiableProperty
{
    public class PropertyModificator<TOwner, TTarget, TProperty> : PropertyModificator<TTarget, TProperty>
    {
        private readonly List<ModificatorPredicatePair> _allModificators = new List<ModificatorPredicatePair>();
        private readonly Dictionary<TOwner, List<ModificatorPredicatePair>> _ownerModificators =
            new Dictionary<TOwner, List<ModificatorPredicatePair>>();

        private readonly Dictionary<TOwner, IDisposable> _disposables =
            new Dictionary<TOwner, IDisposable>();

        public PropertyModificator(
            string targetName,
            float order)
            : base(targetName, (x,y) =>  y, predicate => true, order) { }

        public override bool CanModify(TTarget target)
        {
            return _allModificators.Any(x => x.predicate(target));
        }

        public override bool CanModify(object target)
        {
            return _allModificators.Any(x => x.predicate((TTarget)target));
        }

        public override TProperty Modify(TTarget target, TProperty value)
        {
            var tVal = value;
            foreach(var modificator in _allModificators)
            {
                if(modificator.predicate(target))
                {
                    tVal = modificator.modificator(tVal);
                }
            }
            return tVal;
        }

        public override object Modify(object target, object value)
        {
            return Modify((TTarget)target, (TProperty)value);
        }

        public IDisposable UntilDispose(TOwner owner, Func<TProperty, TProperty> modificator, Func<TTarget, bool> predicate)
        {
            CreateIfNotExist(owner, _ownerModificators);

            var pair = new ModificatorPredicatePair(modificator, predicate);

            _allModificators.Add(pair);
            _ownerModificators[owner].Add(pair);

            IDisposable disposable;

            if(!_disposables.ContainsKey(owner))
            {
                disposable = Disposable.Create(() =>
                {
                    foreach (var modificatorPredicate in _ownerModificators[owner])
                    {
                        _allModificators.Remove(modificatorPredicate);
                    }

                    _ownerModificators[owner].Clear();

                    Disposing(this);

                    PropertiesAndModificators.UpdateProperties<TTarget>(TargetName);
                });

                _disposables.Add(owner, disposable);
            }
            else
            {
                disposable = _disposables[owner];
            }

            PropertiesAndModificators.UpdateProperties<TTarget>(TargetName);

            return disposable;
        }

        public IDisposable UntilDispose(TOwner owner, Func<TProperty, TProperty> modificator)
        {
            return UntilDispose(owner, modificator, x => true);
        }

        static void CreateIfNotExist(TOwner owner, Dictionary<TOwner, List<ModificatorPredicatePair>> modificators)
        {
            if (!modificators.ContainsKey(owner))
            {
                modificators[owner] = new List<ModificatorPredicatePair>();
            }
        }

        private class ModificatorPredicatePair
        {
            public readonly Func<TProperty, TProperty> modificator;
            public readonly Func<TTarget, bool> predicate;

            public ModificatorPredicatePair(Func<TProperty, TProperty> modificator, Func<TTarget, bool> predicate)
            {
                this.modificator = modificator;
                this.predicate = predicate;
            }
        }
    }
}
