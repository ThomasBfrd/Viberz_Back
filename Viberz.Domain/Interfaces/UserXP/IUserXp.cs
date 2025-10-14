using Viberz.Domain.Models;

public interface IUserXp
{
    Task<UserXpInfo> GetUserXp(string userId);
}