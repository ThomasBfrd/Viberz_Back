using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;
using Viberz.Infrastructure.Data;
using Viberz.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> GetUser(string userId)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User> UpdateUser(User user, string userId)
    {
        User? existingUser = await _context.Users.FindAsync(userId);
        if (existingUser == null) throw new Exception("User not found");

        bool userNameExists = await CheckUserName(user);
        if (userNameExists && user.Id != userId) throw new Exception("Username already taken");

        existingUser.Username = user.Username ?? existingUser.Username;
        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.Image = user.Image ?? existingUser.Image;
        existingUser.FavoriteArtists = user.FavoriteArtists ?? existingUser.FavoriteArtists;
        existingUser.FavoriteGenres = user.FavoriteGenres ?? existingUser.FavoriteGenres;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUser(string userId)
    {
        User? existingUser = await _context.Users.FindAsync(userId);
        if (existingUser == null)
        {
            throw new Exception("User not found");
        }
        else
        {
            _context.Users.Remove(existingUser);

            List<XpHistory>? existingXpHistory = await _context.XpHistories.Where(x => x.UserId == userId).ToListAsync();

            _context.XpHistories.RemoveRange(existingXpHistory);

            await _context.SaveChangesAsync();

            return true;
        }
    }

    public async Task<bool> CheckUserName(User user)
    {
        bool usernameExists = await _context.Users.AnyAsync(u => u.Id != user.Id && u.Username!.ToLower() == user.Username.ToLower());

        if (usernameExists) throw new Exception("Username already exists");

        return false;
    }

    public async Task<bool> IsWhitelisted(string email)
    {
        bool isWhitelisted = await _context.Whitelist.AnyAsync(e => e.Email.ToLower() == email.ToLower());

        return isWhitelisted;
    }
}
