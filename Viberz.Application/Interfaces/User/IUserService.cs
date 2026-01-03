using Viberz.Application.DTO.User;

namespace Viberz.Application.Interfaces.User;

public interface IUserService
{
    Task<UserDTO> CreateUser(string jwtToken);
    Task<UserDTO?> GetUserById(string userId);
    Task<UserDTO> UpdateUser(UserUpdateDTO userUpdate, string userId);
    Task<bool> DeleteUser(string userId);
    Task<bool> IsUserWhitelisted(string email);
}
