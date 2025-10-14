using AutoMapper;
using MediatR;
using Viberz.Application.DTO.User;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;

public class UpdateUser : IRequest<UserDTO>
{
    public UserUpdateDTO UserToUpdate { get; }
    public string UserId { get; }

    public UpdateUser(UserUpdateDTO userToUpdate, string userId)
    {
        UserToUpdate = userToUpdate;
        UserId = userId;
    }

    public class Handler : IRequestHandler<UpdateUser, UserDTO>
    {
        private readonly IUserService _userService;

        public Handler(
            IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDTO> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            UserDTO userToUpdate = await _userService.UpdateUser(request.UserToUpdate, request.UserId)
                ?? throw new Exception("Utilisateur non trouvé");

            return userToUpdate;
        }
    }
}
