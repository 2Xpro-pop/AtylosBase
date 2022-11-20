# AtylosBase

AtylosBase is core for game "Atylos". 

# Atylos.ModifiableProperty
Easily create properties with modifiers

### For example

Create class with modifiable properties

```csharp
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
```

Then create class with modificators
```csharp
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
```
Here's how it works
```csharp
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
```

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
