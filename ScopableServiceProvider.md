# Atylos.ScopableServiceProvider
IServiceProvider implementation with the ability to create your own scope. Unlike the Microsoft implementation, services are activated immediately when the scope activated. Only Transient services are activated by "need".

Transient services are always different, a new instance is provided always.

Scoped services are the same within a scope, but different across different scopes.

Singleton services are the same for every scopes.

### For example

Create simple service:
```csharp
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
```
Then use the builder to create the service provider:
```csharp

enum ExampleScopes
{
    C,
    D
}

var builder = new ScopableServiceProviderBuilder();

builder.AddSingleton<Singletone>();

builder.AddTransient<IA, A>();
builder.AddTransient<B>();
builder.AddTransient<DMultiplyer>();

builder.AddScoped<ICScope, CScope>(ExampleScopes.C);
builder.AddScoped<DScope>(ExampleScopes.D);

var services = builder.Build();
```

Then you can activate your scope

```csharp
using(services.ActivateScope(ExampleScopes.C))
{
    using(services.ActivateScope(ExampleScopes.D))
    {
        var d = services.GetService<DScope>();

        Assert.That(d.Bar(), Is.EqualTo(46));
    }
    Assert.Throws<InvalidOperationException>(() =>
    {
        var dm = services.GetService<DMultiplyer>();
    });

}
```

If your service implements IDisposable, the service provider will call Dispose() when the scope completes
