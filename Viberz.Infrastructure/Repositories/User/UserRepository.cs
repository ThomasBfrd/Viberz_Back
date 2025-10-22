using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;
using Viberz.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UserRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> GetUser(string userId)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User> AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user, string userId)
    {
        User? existingUser = await _context.Users.FindAsync(userId);
        if (existingUser == null) throw new Exception("User not found");

        bool userNameExists = await CheckUserName(user);
        if (userNameExists && user.Id != userId) throw new Exception("Username already taken");

        // on détache l'entity pour éviter les conflits de suivi
        _context.Entry(existingUser).State = EntityState.Detached;

        // on attache la nouvelle entity et on marque comme modifiée
        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> CheckUserName(User user)
    {
        bool usernameExists = await _context.Users.AnyAsync(u => u.Id != user.Id && u.Username!.ToLower() == user.Username.ToLower());

        if (usernameExists) throw new Exception("Username already exists");

        return false;
    }
}
