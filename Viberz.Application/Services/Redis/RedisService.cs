using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;
using Viberz.Application.DTO.Songs;

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
        }

        public void AddSongForUser(string userId, string trackId, int ttlMinutes = 3)
        {
            string key = $"user:{userId}:recentSongs";
            _db.SetAdd(key, trackId);
            _db.KeyExpire(key, TimeSpan.FromMinutes(ttlMinutes));
        }

        public void AddPlaylist(string playlistId, SongFromSpotifyPlaylistDTO playlist)
        {
            string key = $"playlist:{playlistId}";
            _db.StringSet(key, JsonSerializer.Serialize(playlist));
            _db.KeyExpire(key, TimeSpan.FromMinutes(5));
        }

        public SongFromSpotifyPlaylistDTO? GetCachedPlaylist(string playlistId)
        {
            string key = $"playlist:{playlistId}";
            RedisValue? cachedData = _db.StringGet(key);

            if (!cachedData.HasValue) return null;
            
            return JsonSerializer.Deserialize<SongFromSpotifyPlaylistDTO>(cachedData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public bool SongAlreadyPlayed(string userId, string trackId)
        {
            string key = $"user:{userId}:recentSongs";
            return _db.SetContains(key, trackId);
        }

        public bool CachedPlaylist(string playlistId)
        {
            string key = $"playlist:{playlistId}";
            return _db.KeyExists(key);
        }
    }
}
