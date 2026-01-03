using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;
using Viberz.Infrastructure.Data;
using Viberz.Infrastructure.Repositories;

public class LikedPlaylistsRepository(ApplicationDbContext context) : BaseRepository<LikedPlaylists, int>(context), ILikedPlaylistsRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task<LikedPlaylists?> GetLikedPlaylistAsync(int playlistId, string userId)
    {
        return await _context.LikedPlaylists
            .FirstOrDefaultAsync(lp => lp.PlaylistId == playlistId && lp.UserId == userId);
    }

    public async Task<bool> IsPlaylistLikedAsync(int playlistId, string userId)
    {
        return await _context.LikedPlaylists.AnyAsync(lp => lp.PlaylistId == playlistId && lp.UserId == userId);
    }
}
