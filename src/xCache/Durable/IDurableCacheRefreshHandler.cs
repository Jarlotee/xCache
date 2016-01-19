namespace xCache.Durable
{
    public interface IDurableCacheRefreshHandler
    {
        void Handle(DurableCacheRefreshEvent refreshEvent);
    }
}