using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Viberz.Application.Services.Redis
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConfiguration config)
        {
            var redisSection = config.GetSection("Redis");
            var options = new ConfigurationOptions
            {
                EndPoints = { $"{redisSection["Host"]}:{redisSection["Port"]}" },
                User = redisSection["User"],
                Password = redisSection["Password"]
            };

            _redis = ConnectionMultiplexer.Connect(options);
            _db = _redis.GetDatabase();

            _db.StringSet("foo", "bar");
        }

        public void AddSongForUser(string userId, string trackId, int ttlMinutes = 3)
        {
            string key = $"user:{userId}:recentSongs";
            _db.SetAdd(key, trackId);
            _db.KeyExpire(key, TimeSpan.FromMinutes(ttlMinutes));
        }

        public bool SongAlreadyPlayed(string userId, string trackId)
        {
            string key = $"user:{userId}:recentSongs";
            return _db.SetContains(key, trackId);
        }
    }
}
