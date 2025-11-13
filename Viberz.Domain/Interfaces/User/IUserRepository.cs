using Viberz.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetUser(string userId);
    Task<User> AddUser(User user);
    Task<User> UpdateUser(User user, string userId);
    Task<bool> DeleteUser(string userId);
    Task<bool> CheckUserName(User user);
    Task<bool> IsWhitelisted(string email);
}
