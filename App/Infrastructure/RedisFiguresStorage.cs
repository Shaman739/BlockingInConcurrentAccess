using BlockingResourcesInConcurrentAccess.Core.Contract;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace BlockingResourcesInConcurrentAccess.Infrastructure
{
    public class RedisFiguresStorage : IFiguresStorage
    {
        private const string LOCKED_PREFIX = "locked";
        public const string CIRCLE = "Circle";
        public const string SQUARE = "Square";
        public const string TRIANGLE = "Triangle";

        private Lazy<IDatabase> _db;
        private readonly ILogger<IFiguresStorage> _logger;

        public RedisFiguresStorage(ILogger<IFiguresStorage> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            Init(connectionMultiplexer);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public void Init(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = new Lazy<IDatabase>(() =>
            {
                if (connectionMultiplexer == null) throw new ArgumentNullException(nameof(connectionMultiplexer));

                var dbRedis = connectionMultiplexer.GetDatabase();

                dbRedis.KeyDelete(CIRCLE);
                dbRedis.KeyDelete(SQUARE);
                dbRedis.KeyDelete(TRIANGLE);

                dbRedis.KeyDelete($"{LOCKED_PREFIX}_{CIRCLE}");
                dbRedis.KeyDelete($"{LOCKED_PREFIX}_{SQUARE}");
                dbRedis.KeyDelete($"{LOCKED_PREFIX}_{TRIANGLE}");

                dbRedis.StringSet(new RedisKey(CIRCLE), new RedisValue(100.ToString()));
                dbRedis.StringSet(new RedisKey(SQUARE), new RedisValue(100.ToString()));
                dbRedis.StringSet(new RedisKey(TRIANGLE), new RedisValue(100.ToString()));



                return dbRedis;
            });


        }
        public async Task<(bool isSuccess, string blockingId)> Get(string type, int count)
        {
            await Task.Delay(100);

            string guid = Guid.NewGuid().ToString();
            _db.Value.HashSet(LOCKED_PREFIX, new HashEntry[] { new HashEntry(new RedisValue(guid), new RedisValue(count.ToString())) });
            string script = $"local sum = 0 local i = 1 local a1 = redis.call('hvals', KEYS[1]) while (a1[i]) do sum = sum + a1[i] i = i + 1 end if tonumber(redis.call('get', KEYS[2])) > sum then redis.call('hset', KEYS[1], KEYS[3], KEYS[4]) return 1 else return 0 end";

            RedisKey[] keys = { $"{LOCKED_PREFIX}_{type}", type, guid, count.ToString() };

            var result = _db.Value.ScriptEvaluate(script, keys);
            return result.ToString() == "1" ? (true, guid) : (false, string.Empty);
        }

        public async Task Set(string type, int current, string guidBlockingData)
        {
            _logger.LogWarning($"Уменьшаем на {current}");


            var typeKey = new RedisKey(type);
            await _db.Value.StringDecrementAsync(typeKey, current);

            RedisKey keys = new RedisKey($"{LOCKED_PREFIX}_{type}");

            await _db.Value.HashDeleteAsync(keys, guidBlockingData);
            _logger.LogWarning($"Остаток {_db.Value.StringGet(typeKey)}");
        }

    }
}
