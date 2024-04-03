using StackExchange.Redis;

namespace RedisSample.Extensions
{
    public static class StackExchangeRedisExtension
    {
        public static void AddStackExchangeRedis(this IServiceCollection services, IConfiguration configuration)
        {
            string _redisHost = configuration["Redis:Host"];
            string _redisPort = configuration["Redis:Port"];
            var configString = $"{_redisHost}:{_redisPort}";
            var redis = ConnectionMultiplexer.Connect(configString);
            services.AddSingleton(redis); 
        }
    }
}
