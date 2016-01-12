using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using xCache.Aop.Unity;
using xCache.Durable;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityCacheEnabledTests : CacheEnabledTests
    {
        IUnityContainer _container;

        public UnityCacheEnabledTests()
        {    
            _container = new UnityContainer();

            //Register interception
            _container.AddNewExtension<Interception>();

            //Register xCache
            _container.RegisterType<ICache, MemoryCache>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDurableCacheQueue, TimedDurableCacheQueue>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDurableCacheRefreshHandler, DurableCacheRefreshHandler>(new ContainerControlledLifetimeManager());

            //Register test class with interception
            _container.RegisterType<ICacheEnableObject, UnityCacheEnabledObject>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            _cached = _container.Resolve<ICacheEnableObject>();
        }
    }
}
