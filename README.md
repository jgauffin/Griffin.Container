Griffin.Container
=================

Inversion of control container with (almost) zero configuration.

The container configuration is validated when the container is built, to make sure that all dependencies have been registered.

Support for .NET Standard 2.0 and .NET 4.5.2.

# Typical setup:

1. Add the `[Component]` attribute on your classes:

```csharp
[Component]
public class MyService : IShoppingService
{
    public MyService(IShoppingRepository repos)
	{
	}
	
	// [...]
}

[Component]
public class SqlServerShoppingRepository : IShoppingRepository
{
    public SqlServerShoppingRepository()
	{
	}
	
	// [...]
}
```

2. Register all decorated classes in the container:

```csharp
var registrar = new ContainerRegistrar();
registrar.RegisterComponents(Lifetime.Scoped, Assembly.GetExecutingAssembly());

var container = registrar.Build();
```



# Features

* The usual =)
* Configuration validation
* Commands
* Decorators
* Interceptors
* Modules
* Domain events
* [Quick enough](http://www.palmmedia.de/Blog/2011/8/30/ioc-container-benchmark-performance-comparison)

# Installation

Install using nuget:

* Core: `install-package griffin.container`
* MVC5: `install-package griffin.container.mvc5`
* WCF: `install-package griffin.container.wcf`
* WebApi v2: `install-package griffin.container.webapi2`

# Documentation

* The article linked above
* Core [MSDN style docs](http://griffinframework.net/docs/container/)
* WCF [MSDN style docs](http://griffinframework.net/docs/container/wcf)
* [Forum/Mailing list](https://groups.google.com/forum/#!forum/griffin-container)

Pull requests are welcome.
