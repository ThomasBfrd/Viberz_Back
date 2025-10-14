using Microsoft.EntityFrameworkCore;
using Viberz.Application.DTO.User;
using Viberz.Domain.Entities;
using Viberz.Viberz.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUser(string userId)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User> AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CheckUserName(string userName)
        => await _context.Users.AnyAsync(u => u.Username!.ToLower() == userName.ToLower());
}
