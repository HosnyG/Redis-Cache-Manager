using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedisCacheManager
{
    public interface IRedisCacheManager
    {

        bool IsConnected();

        void CloseConnection();

        Task SetObjAsync<T>(string key, T obj, When? whenSet = null, int? ttl = null);

        void SetObj<T>(string key, T obj, When? whenSet = null, int? ttl = null);

        Task<T> GetObjAsync<T>(string key);
        T GetObj<T>(string key);
        bool IsKeyExist(string key);

        Task<bool> IsKeyExistAsync(string key);

        void RemoveKey(string key);

        Task RemoveKeyAsync(string key);
        int? KeyTTL(string key);
    }
}
