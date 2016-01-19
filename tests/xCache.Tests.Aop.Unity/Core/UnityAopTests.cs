using System.Collections;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using xCache.Aop.Unity;
using xCache.Tests.Core;

namespace xCache.Tests.Aop.Unity.Core
{
    public class UnityAopTests : AopTests
    {
        IUnityContainer _container;

        public UnityAopTests()
        {    
            DictionaryCache.Clear();
            _container = new UnityContainer();

            //Register interception
            _container.AddNewExtension<Interception>();

            //Register xCache
            _container.RegisterType<ICache, MemoryCache>(new InjectionConstructor(true));
            _container.RegisterType<ICache, DictionaryCache>("Dictionary", new InjectionFactory(x =>
                new DictionaryCache(DictionaryCache, _container.Resolve<ICache>(), true)));
            _container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>();

            //Register test class with interception
            _container.RegisterType<IAop, UnityAop>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            _aop = _container.Resolve<IAop>();
        }
    }
}
