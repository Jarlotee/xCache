# xCache

xCache is a decoupled caching abstraction for .NET. 

xCache comes with support for System.Runtime.Caching.MemoryCache out of the box and is designed to be easy to extend with other caching implementations.

## Installation

The [core library] is available on nuget 

`install-package xCache`

## Usage

> Register your preffered caching service to the ICache interface through your DI container.

```csharp

	//Register
	Ioc.RegisterType<ICache, xCache.MemoryCache>();

```

> Resolve ICache and handle your caching needs

```csharp
	
	var cache = Ioc.Resolve<ICache>();
	var key = "testKey";
	
	var result = cache.Get<string>(key);
	
	if(result == null)
	{
		result = GetResult();
		
		//Cache for 5 minutes
		cache.Add<string>(key, result, new Timespan(0,5,0));
	}
```

## Aop

Because caching is a cross-cutting concern it can be executed with some flavor of Aspect oriented programming (Aop) to seperate concerns. 

> xCache.Aop.Unity - Interface interception and attribute caching

xCache.Aop.Unity scaffolds the IoC interception pattern and provides a caching attribute to enable caching functionality on a method by method basis

### Installation

The [Aop Unity] package is available on nuget 

`install-package xCache.Aop.Unity`

### Usage

```csharp

//Create an interface for interception
public interface IAop
{
    string GetCurrentDateAsStringFiveSecondCache();
}

//Impliment that interface

public class UnityAop : IAop
{
    [Cache(Seconds = 5)]
    public string GetCurrentDateAsStringFiveSecondCache()
    {
        return DateTime.Now.ToString();
    }
}

```

```csharp

var container = new UnityContainer();

//Register interception
container.AddNewExtension<Interception>();

//Register xCache
container.RegisterType<ICache, xCache.MemoryCache>();
container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>();

//Register test interface with interception
container.RegisterType<IAop, UnityAop>(
    new InterceptionBehavior<PolicyInjectionBehavior>(),
    new Interceptor<InterfaceInterceptor>());

//Resolve
var aop = _container.Resolve<IAop>();

//Should return the same DateTime string for 5 second intervals
var cachedDateTimeString = aop.GetCurrentDateAsStringFiveSecondCache();

```


### Version
0.1.0

### License
MIT

### Potential Future Features
* Handle Out Parameters caching
* Add Reflection based extensions method to configure objects that use CacheAttribute in Aop.Unity

[core library]:https://www.nuget.org/packages/xCache/
[Aop Unity]:https://www.nuget.org/packages/xCache.Aop.Unity/