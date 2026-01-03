using Viberz.Domain.Entities;
using Viberz.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User, string>
{
    Task<User?> GetUser(string userId);
    Task<User> UpdateUser(User user, string userId);
    Task<bool> DeleteUser(string userId);
    Task<bool> CheckUserName(User user);
    Task<bool> IsWhitelisted(string email);
}
