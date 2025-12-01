using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using Viberz.Application.Commands.User;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.User;
using Viberz.Application.Queries.User.IsWhitelisted;
using Viberz.Application.Utilities;

namespace Viberz.API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(
    JwtDecode jwtDecode,
    IMediator mediator) : ControllerBase
{
    private readonly JwtDecode _jwtDecode = jwtDecode;
    private readonly IMediator _mediator = mediator;

    [HttpGet("me")]
    public async Task<IActionResult> GetUsers()
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        UserDTO? user = await _mediator.Send(new GetUserQuery(token.UserId));

        if (user is null)
        {
            UserDTO newUser = await _mediator.Send(new CreateUser(token.SpotifyJwt));

            return Ok(newUser);
        }

        return Ok(user);
    }
    [HttpPut("me")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userToUpdate)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
        throw new Exception("User informations are missing in the JWT.");

        UserDTO user = await _mediator.Send(new UpdateUser(userToUpdate, token.UserId));
        return Ok(user);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser([FromBody] string userId)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        bool result = await _mediator.Send(new DeleteUser(token.UserId));

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("is-whitelisted")]
    public async Task<IActionResult> IsUserWhitelisted([FromBody] string email)
    {
        bool isWhitelisted = await _mediator.Send(new IsWhitelistedQuery(email));

        return Ok(isWhitelisted);
    }
}