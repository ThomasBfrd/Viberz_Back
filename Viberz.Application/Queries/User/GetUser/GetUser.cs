using MediatR;
using Viberz.Application.Interfaces.User;

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
    private readonly IUserService _userService;

    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDTO?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserDTO? user = await _userService.GetUserById(request.UserId);

        return user;
    }
}
