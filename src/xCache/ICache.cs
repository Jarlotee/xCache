namespace xCache
{
    public interface ICache
    {
        void Add<T>(string key, CacheItem<T> item);
        CacheItem<T> Get<T>(string key);
        bool Remove(string key);
        void RemoveAll();
    }
}
