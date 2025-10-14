using MediatR;
using Viberz.Application.DTO.Xp;
using Viberz.Domain.Entities;

public class GetUserQuery : IRequest<UserDTO?>
{
    public string UserId { get; }
    public GetUserQuery(string userId)
    {
        UserId = userId;
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO?>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserXp _userXp;

    public GetUserQueryHandler(IUserRepository userRepository, IUserXp userXp)
    {
        _userRepository = userRepository;
        _userXp = userXp;
    }

    public async Task<UserDTO?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUser(request.UserId);

        UserXpDTO? userXp = await _userXp.GetUserXp(request.UserId);

        if (user is not null && userXp is not null)
        {
            return new()
            {
                User = user,
                Xp = userXp
            };
        }

        return null;
    }
}
