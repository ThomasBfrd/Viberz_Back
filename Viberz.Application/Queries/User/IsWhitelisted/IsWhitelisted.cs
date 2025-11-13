using MediatR;
using Viberz.Application.Interfaces.User;

namespace Viberz.Application.Queries.User.IsWhitelisted;

public class IsWhitelistedQuery : IRequest<bool>
{
    public string Email { get; }
    public IsWhitelistedQuery(string email)
    {
        Email = email;
    }
}

public class IsWhitelistedHandler : IRequestHandler<IsWhitelistedQuery, bool>
{
    private readonly IUserService _userService;
    public IsWhitelistedHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<bool> Handle(IsWhitelistedQuery request, CancellationToken cancellationToken)
    {
        return await _userService.IsUserWhitelisted(request.Email);
    }
}
