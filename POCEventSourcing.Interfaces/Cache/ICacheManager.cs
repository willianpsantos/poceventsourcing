namespace POCEventSourcing.Interfaces.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key);

        bool Remove(string key);
        bool Store<T>(string key, T value, TimeSpan? expire = null);
    }
}
