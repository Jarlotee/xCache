using Unity.Interception.PolicyInjection.Pipeline;

namespace xCache.Aop.Unity
{
    public interface ICacheKeyGenerator
    {
        string GenerateKey(IMethodInvocation invocation);
    }
}
