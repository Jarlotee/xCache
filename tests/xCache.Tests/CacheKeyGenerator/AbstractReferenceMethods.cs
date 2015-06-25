namespace xCache.Tests.CacheKeyGenerator
{
    public abstract class AbstractReferenceMethods
    {
        public virtual string VirtualMethod(int i)
        {
            return i.ToString();
        }

        public virtual string OverriddenVirtualMethod(int i)
        {
            return i.ToString();
        }

        protected string InheritedProtectedMethod(int i)
        {
            return (i + 2).ToString();
        }
    }
}
