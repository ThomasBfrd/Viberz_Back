using Viberz.Application.DTO.User;
using Viberz.Domain.Entities;

namespace Viberz.Application.Interfaces.User;

public interface IUserService
{
    Task<UserDTO> CreateUser(string jwtToken);
    Task<UserDTO?> GetUserById(string userId);
    Task<UserDTO> UpdateUser(UserUpdateDTO userUpdate, string userId);
}
