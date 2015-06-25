namespace xCache.Tests.CacheKeyGenerator
{
    public abstract class AbstractComplexObject
    {
        public virtual string AbstractString { get; set; }

        public AbstractComplexObject()
        {
            AbstractString = "Virtual";
        }
    }
}
