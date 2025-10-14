using Viberz.Application.DTO.Xp;
using Viberz.Domain.Entities;

public class UserDTO
{
    public User User { get; set; } = new();
    public UserXpDTO Xp { get; set; } = new();

}
