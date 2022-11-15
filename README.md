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
