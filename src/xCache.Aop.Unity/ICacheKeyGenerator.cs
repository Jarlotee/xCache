using Microsoft.Practices.Unity.InterceptionExtension;

namespace xCache.Aop.Unity
{
    public interface ICacheKeyGenerator
    {
        string GenerateKey(IMethodInvocation invocation);
    }
}
