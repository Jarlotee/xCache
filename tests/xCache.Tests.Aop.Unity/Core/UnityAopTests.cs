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
            _container = new UnityContainer();

            //Register interception
            _container.AddNewExtension<Interception>();

            //Register xCache
            _container.RegisterType<ICache, xCache.MemoryCache>();
            _container.RegisterType<ICacheKeyGenerator,JsonCacheKeyGenerator>();

            //Register test class with interception
            _container.RegisterType<IAop, UnityAop>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            _aop = _container.Resolve<IAop>();
        }
    }
}
