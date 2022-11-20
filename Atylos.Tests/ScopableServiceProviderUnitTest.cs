using Atylos.ScopableServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atylos.Tests
{
    public class ScopableServiceProviderUnitTest
    {
        private ScopableServiceProvider.PoorScopableServiceProvider services;

        [SetUp]
        public void Setup()
        {
            var builder = new ScopableServiceProviderBuilder();

            builder.AddSingleton<Singletone>();

            builder.AddTransient<IA, A>();
            builder.AddTransient<B>();
            builder.AddTransient<DMultiplyer>();

            builder.AddScoped<ICScope, CScope>(ExampleScopes.C);
            builder.AddScoped<DScope>(ExampleScopes.D);

            services = builder.Build();
        }

        [Test]
        public void ScopeTest()
        {
            using(services.ActivateScope(ExampleScopes.C))
            {
                using (services.ActivateScope(ExampleScopes.D))
                {
                    var d = services.GetService<DScope>();

                    Assert.That(d.Bar(), Is.EqualTo(46));
                }
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var dm = services.GetService<DMultiplyer>();
                });
                
            }

        }

        enum ExampleScopes
        {
            C,
            D
        }

        class Singletone { public int Foo => 2; }

        interface IA
        {
            int Foo();
        }

        class A : IA
        {
            public int Foo() => 11;
        }

        class B { public int Foo() => 12; }
        interface ICScope { int Bar(); }
        class CScope : ICScope
        {
            private IA _a;
            private B _b;

            public CScope(IA a, B b)
            {
                _a = a;
                _b = b;
            }

            public int Bar()
            {
                return _a.Foo() + _b.Foo();
            }
        }

        class DScope
        {
            private ICScope _cScope;
            private Singletone _singletone;

            public DScope(ICScope cScope, Singletone singletone)
            {
                _cScope = cScope;
                _singletone = singletone;
            }

            public int Bar()
            {
                return _cScope.Bar() * _singletone.Foo;
            }
        }

        class DMultiplyer
        {
            private DScope _dScope;

            public DMultiplyer(DScope dScope)
            {
                _dScope = dScope;
            }

            public int Boo()
            {
                return _dScope.Bar();
            }
        }
    }
}
