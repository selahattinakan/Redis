using StackExchange.Redis;

namespace RedisSample.Services
{
    public class StackExchangeRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        public StackExchangeRedisService(ConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
