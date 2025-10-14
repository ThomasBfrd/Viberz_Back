using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;
using Viberz.Domain.Interfaces.Genres;
using Viberz.Infrastructure.Data;

namespace Viberz.Infrastructure.Repositories.Genres;

public class GenresRepository : IGenresRepository
{
    private readonly ApplicationDbContext _context;

    public GenresRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetAllGenres()
    {
        return await _context.Genres.Select(g => g.Name).ToListAsync();
    }

    public async Task<List<Genre>> GetAllGenresWithSpotifyId()
    {
        return await _context.Genres.ToListAsync();
    }
}
