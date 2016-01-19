namespace xCache.Durable
{
    public interface IDurableCacheQueue
    {
        void ScheduleRefresh(DurableCacheRefreshEvent refreshEvent);
        void Purge();
    }
}
