using Viberz.Application.DTO.Xp;

public interface IUserXp
{
    Task<UserXpDTO> GetUserXp(string userId);
}