using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Domain.Extensions;
using Viberz.Infrastructure.Data;
using Viberz.Infrastructure.Repositories;

public class PlaylistRepository(ApplicationDbContext context) : BaseRepository<Playlist, int>(context), IPlaylistRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> DeletePlaylist(string playlistId)
    {
        if (string.IsNullOrEmpty(playlistId)) throw new ArgumentException("Playlist ID cannot be null or empty.", nameof(playlistId));

        Playlist? playlist = await _context.Playlists.FindAsync(playlistId);

        if (playlist == null) throw new ArgumentException("Playlist not found.", nameof(playlistId));

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Playlist>> GetPlaylists(int page = 1, int pageSize = 5)
    {
        List<Playlist>? playlists = await _context.Playlists
            .OrderByDescending(p => p.Likes)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (playlists == null || playlists.Count == 0) return [];

        return playlists;
    }

    public async Task<List<Playlist>> GetPlaylistsByFamily(string userId, FamilyGenres family, string search, bool likes, int page = 1, int pageSize = 5)
    {
        IQueryable<Playlist> query;
        string genre = family.GetDisplayName();

        if (genre == "All" || genre == "Owner")
        {
            query = _context.Playlists;

        }
        else
        {
            query = _context.Playlists
                .Where(p => p.GenreList.Any(e => e.Equals(family.GetDisplayName())));
        }

        if (likes)
        {
            query = query
                .Where(p => _context.LikedPlaylists
                    .Any(lp => lp.PlaylistId.Equals(p.Id))
                    && _context.LikedPlaylists.Any(lp2 => lp2.UserId.Equals(userId)));
        }

        if (genre == "Owner")
        {
            query = query.Where(p => p.UserId.Equals(userId));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(s => s.Name.Contains(search) || _context.Users.Any(u => u.Id.Equals(s.UserId) && u.Username.Contains(search)));
        }

        List<Playlist>? playlists = await query
            .OrderByDescending(p => p.Likes)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return playlists.Count == 0 ? [] : playlists;
    }

    public async Task<bool> LikePlaylist(int playlistId, string userId)
    {
        Playlist? playlist = await _context.Playlists.FindAsync(playlistId) ??
            throw new ArgumentException("Playlist not found.", nameof(playlistId));

        User? existingUser = await _context.Users.FindAsync(userId) ??
            throw new ArgumentException("User not found.", nameof(userId));

        playlist.Likes += 1;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DislikePlaylist(int playlistId, string userId)
    {
        Playlist? playlist = await _context.Playlists.FindAsync(playlistId) ??
            throw new ArgumentException("Playlist not found.", nameof(playlistId));

        User? existingUser = await _context.Users.FindAsync(userId) ??
            throw new ArgumentException("User not found.", nameof(userId));

        playlist.Likes -= 1;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Playlist> UpdatePlaylist(Playlist updatedPlaylist)
    {
        Playlist? playlist = await _context.Playlists.FindAsync(updatedPlaylist.Id) ??
            throw new ArgumentException("Playlist not found.", nameof(updatedPlaylist.Id));

        playlist.Name = updatedPlaylist.Name;
        playlist.GenreList = updatedPlaylist.GenreList;
        playlist.SpotifyPlaylistId = updatedPlaylist.SpotifyPlaylistId;

        await _context.SaveChangesAsync();

        return playlist;
    }
}
