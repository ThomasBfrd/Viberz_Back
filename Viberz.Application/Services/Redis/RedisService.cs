using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;
using Viberz.Application.DTO.Songs;
using Viberz.Domain.Enums;

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

        public void AddSongForUser(string userId, string trackId, Activies context, int ttlMinutes)
        {
            string key = $"user:{userId}:{context}:recentSongs";
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

        public bool SongAlreadyPlayed(string userId, string trackId, Activies context)
        {
            string key = $"user:{userId}:{context}:recentSongs";
            return _db.SetContains(key, trackId);
        }

        public bool CachedPlaylist(string playlistId)
        {
            string key = $"playlist:{playlistId}";
            return _db.KeyExists(key);
        }
    }
}
