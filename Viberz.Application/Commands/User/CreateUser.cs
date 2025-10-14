using MediatR;
using Viberz.Application.Interfaces.User;

public class CreateUser : IRequest<UserDTO>
{
    public string AccessToken { get; }

    public CreateUser(string accessToken)
    {
        AccessToken = accessToken;
    }

    // Le Handler interne gère toute la logique métier
    public class Handler : IRequestHandler<CreateUser, UserDTO>
    {
        // Injection des dépendances nécessaires
        private readonly IUserService _userService;

        public Handler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDTO> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUser(request.AccessToken);
        }
    }
}
