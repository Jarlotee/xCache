using System;

namespace xCache
{
    public interface ICache
    {
        void Add<T>(string key, T item, TimeSpan expiration);
        T Get<T>(string key);
    }
}
