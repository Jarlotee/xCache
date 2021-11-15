using System;
using xCache.Aop.Unity;
using xCache.Aop.Unity.Durable;
using xCache.Durable;
using xCache.Tests.Core;
using Unity;
using Unity.Interception;
using Unity.Lifetime;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Interception.PolicyInjection;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityCacheEnabledTests : CacheEnabledTests
    {
        public UnityCacheEnabledTests()
        {    
            _container = new UnityContainer();

            //Register interception
            _container.AddNewExtension<Interception>();

            //Register xCache
            _container.RegisterType<ICache, MemoryCache>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, MemoryCache>("One", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, MemoryCache>("Two", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICache, DictionaryCache>("DictionaryCache", new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDurableCacheRefreshHandler, UnityDurableCacheRefreshHandler>(new ContainerControlledLifetimeManager());

            _container.RegisterFactory<IDurableCacheQueue>((c) => new TimedDurableCacheQueue(c.Resolve<IDurableCacheRefreshHandler>(), new TimeSpan(0, 0, 30)), new ContainerControlledLifetimeManager());

            //Register test class with interception
            _container.RegisterType<ICacheEnableObject, UnityCacheEnabledObject>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            // The default behavior of MemoryCache is static so clear it before each test.
            var memoryCache = _container.Resolve<ICache>();
            memoryCache.RemoveAll();

            _cached = _container.Resolve<ICacheEnableObject>();
        }

        protected override void PurgeDurableCacheQueue()
        {
            var queue = _container.Resolve<IDurableCacheQueue>();
            queue.Purge();
        }

        protected override void PurgeDictionaryCache()
        {
            //TODO figure out how to dispose this through unity
            var dictionary = (DictionaryCache)_container.Resolve<ICache>("DictionaryCache");
            dictionary.RemoveAll();
        }
    }
}
