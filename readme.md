# xCache

xCache is a decoupled caching abstraction for .NET. 

xCache comes with support for System.Runtime.Caching.MemoryCache out of the box and is designed to be easy to extend with other caching implementations. Like the [xCache.Redis] implementation.

## Installation

The [core library] is available on nuget 

`install-package xCache`

## Usage

> Register your preffered caching service to the ICache interface through your DI container.

```csharp

	//Register
	Ioc.RegisterType<ICache, MemoryCache>();

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

Because caching is a cross-cutting concern it can be executed with some flavor of Aspect oriented programming (Aop) to promote single responsiblity. 

xCache.Aop.Unity scaffolds the IoC interception pattern and provides a caching attribute to enable caching functionality on a method by method basis.

It comes bundled with a Json.Net cache key generator that leverage generic serialization to ensure unique keys are always generated.

### Installation

The [Aop Unity] package is available on nuget 

`install-package xCache.Aop.Unity`

### Usage

```csharp

//Create an interface for interception
public interface ICachedObject
{
    string GetCurrentDateAsStringFiveSecondCache();
}

//Impliment that interface

public class UnityCachedObject : ICachedObject
{
    [Cache(Seconds = 5)]
    public string GetCurrentDateAsStringFiveSecondCache()
    {
        return DateTime.Now.ToString();
    }
}

```

```csharp

public class CacheTests 
{
	[Fact]
	public void FiveSecondTest()
	{
		var container = new UnityContainer();
		
		//Register interception
		container.AddNewExtension<Interception>();
		
		//Register xCache
		container.RegisterType<ICache, xCache.MemoryCache>();
		container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>();
		
		//Register test interface with interception
		container.RegisterType<ICachedObject, UnityCachedObject>(
		    new InterceptionBehavior<PolicyInjectionBehavior>(),
		    new Interceptor<InterfaceInterceptor>());
		
		//Resolve
		var obj = container.Resolve<ICachedObject>();
		
		//Should return the same DateTime string for 5 second intervals
		var cachedDateTimeString = obj.GetCurrentDateAsStringFiveSecondCache();
	}
}

```

## Durable Cache

As of xCache.Aop.Unity@0.3.0 you now have the ability to refresh your cache from a background processes without relying on your method being called. To opt into this new feature you will need to register two additional objects:

```csharp
		//Register xCache Durable Dependencies
		container.RegisterType<IDurableCacheQueue, TimedDurableCacheQueue>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory((c) => new TimedDurableCacheQueue(c.Resolve<IDurableCacheRefreshHandler>(), new TimeSpan(0,0,30))));
		container.RegisterType<IDurableCacheRefreshHandler, DurableCacheRefreshHandler>();
```
And add Absolute expiration values to the Cache attribute like so:

```csharp
	//Cache for 90 seconds refreshing every 15 from the cache queue
	[Cache(Seconds = 15, AbsoluteSeconds = 90)]
```

When you add an Absolute[Hours|Minutes|Seconds] value you are setting the maximum lifetime for the item to be cached, and using the Seconds, Minutes, or Hours attribute to indicate how often your item should be refreshed within that lifetime.

It is important to note that in the absence of an Absolute value, the Seconds, Minutes and Hours attribute will continue to act as cache expiration times and will only be updated when a cache miss occurs.

The TimedDurableCacheQueue will schedule a refresh of cached items using .NET timers to ensure that your items are refreshed at the specified intevals. It can be overridden with your own prefered implementation.

The DurableCacheRefreshHandler interogates your code to find the correct method and paramters needed to refresh the cache and executed on a seperate background thread.

### Automatic Rescheduling

Every time an item is retrieved from durable cache it is checked for staleness. By default a stale itme is defined as an item that has not been updated for at least two refresh cycles. You can customize the maximum staleness by setting the MaximumStaleness[Hours|Minutes|Seconds] values.

## Caching Tiers
As of xCache.Aop.Unity@0.4.0 you can now specify multiple caching tiers each with thier own lifetimes.

You will need to overload the registration of ICache with your desired caching implementations:

```csharp
//Register multiple tiers by name
container.RegisterType<ICache, MemoryCache>();
container.RegisterType<ICache, RedisCache>("Redis");
```

Then add the appropriate cache attributes:

```csharp
[Cache(Minutes = 1, Order = 1)]
[Cache(Minutes = 30, CacheName = "Redis", Order = 2)]
public string GetCurrentDateAsString()
{
	return DateTime.Now.ToString();
}
```

### Version
* xCache 0.2.2
* xCache.Aop.Unity 0.5.0

### License
MIT

[core library]:https://www.nuget.org/packages/xCache/
[Aop Unity]:https://www.nuget.org/packages/xCache.Aop.Unity/
[xCache.Redis]:https://github.com/Jarlotee/xCache.Redis
