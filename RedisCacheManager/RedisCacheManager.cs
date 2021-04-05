using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheManager
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private IConnectionMultiplexer _muxer;
        private readonly string _keysPrefix;
        private IDatabase _redisCache;

        public RedisCacheManager(IConnectionMultiplexer muxer, string keysPrefix)
        {
            this._muxer = muxer;
            this._keysPrefix = keysPrefix;
            if (_muxer != null && _muxer.IsConnected)
                _redisCache = _muxer.GetDatabase();
        }

        public bool IsConnected()
        {
            return _muxer != null && _muxer.IsConnected;
        }

        public void CloseConnection()
        {
            _muxer.Close();
        }

        public async Task SetObjAsync<T>(string key, T obj, When? whenSet = null, int? ttl = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            When _when = whenSet ?? When.Always;
            TimeSpan? expiresAfterSeconds = null;
            if (ttl != null)
                expiresAfterSeconds = TimeSpan.FromSeconds(ttl.Value);
            await _redisCache.StringSetAsync(_keysPrefix + "_" + key, json, when: _when, expiry: expiresAfterSeconds);
        }

        public void SetObj<T>(string key, T obj, When? whenSet = null, int? ttl = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            When _when = whenSet ?? When.Always;
            TimeSpan? expiresAfterSeconds = null;
            if (ttl != null)
                expiresAfterSeconds = TimeSpan.FromSeconds(ttl.Value);
            _redisCache.StringSet(_keysPrefix + "_" + key, json, when: _when, expiry: expiresAfterSeconds);
        }

        public async Task<T> GetObjAsync<T>(string key)
        {
            string json = await _redisCache.StringGetAsync(_keysPrefix + "_" + key);
            if (json is null)
                return default;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T GetObj<T>(string key)
        {
            string json = _redisCache.StringGet(_keysPrefix + "_" + key);
            if (json is null)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool IsKeyExist(string key)
        {
            return _redisCache.KeyExists(_keysPrefix + "_" + key);
        }

        public async Task<bool> IsKeyExistAsync(string key)
        {
            return await _redisCache.KeyExistsAsync(_keysPrefix + "_" + key);
        }

        public void RemoveKey(string key)
        {
            _redisCache.KeyDelete(_keysPrefix + "_" + key);
        }

        public async Task RemoveKeyAsync(string key)
        {
            await _redisCache.KeyDeleteAsync(_keysPrefix + "_" + key);
        }

        //in seconds
        public int? KeyTTL(string key)
        {
            TimeSpan? ttl = _redisCache.KeyTimeToLive(_keysPrefix + "_" + key);
            if (ttl == null)
                return null;
            return (int)Math.Ceiling(ttl.Value.TotalSeconds);
        }
    }
}
