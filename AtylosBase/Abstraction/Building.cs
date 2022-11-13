using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;

namespace AtylosBase.Abstraction
{
    public abstract class Building
    {
        private List<IDisposable> _effects = new List<IDisposable>(5);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void OnBuild();
        public virtual void OnDestroy()
        {
            IObservable<AtylosEvent> observable = new Subject<AtylosEvent>();

            DisposeWhenDestory(
                observable.Subscribe(atylos => Console.WriteLine("YEEE")),
                observable.Subscribe(atylos => Console.WriteLine("BOY"))
            );

            foreach(var effect in _effects)
            {
                effect.Dispose();
            }
        }

        protected void DisposeWhenDestory(params IDisposable[] disposable) => _effects.AddRange(disposable);
    }
}
