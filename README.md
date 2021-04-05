# Redis-Cache-Manager
Redis Cache Manager in C# , Using StackExchange.Redis v2.2.4 library


Manage Redis functionality in C# , Serializing and Deserializing C# objects and put in Redis Cache


using this manager is ASP .NET Core:

```
string redisIP = "127.0.0.1";
int redisPort = 6379; 
string keysPrefix = "Dev"
ConfigurationOptions cacheOptions = new ConfigurationOptions
{
    EndPoints ={
        {redisIP,redisPort}
    },
    AbortOnConnectFail = false
};
services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(cacheOptions));
services.AddScoped<IRedisCacheManager>(s => new RedisCacheManager(s.GetRequiredService<IConnectionMultiplexer>(), keysPrefix));
```
