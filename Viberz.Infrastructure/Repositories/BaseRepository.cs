using Microsoft.EntityFrameworkCore;
using Viberz.Infrastructure.Data;

namespace Viberz.Infrastructure.Repositories;

public abstract class BaseRepository<T, TId>(ApplicationDbContext context)
       where T : class
{
    private readonly ApplicationDbContext _context = context;
    protected DbSet<T> DbSet => _context.Set<T>();

    public async Task Add(T entity)
    {
        await DbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        DbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(TId id)
    {
        return await DbSet.FindAsync(id);
    }
}
