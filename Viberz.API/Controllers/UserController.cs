using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> GetUser([FromHeader] string userId)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        UserDTO? user = await _mediator.Send(new GetUserQuery(userId));

        if (user is null && userId == token.UserId)
        {
            return CreateUser(token.SpotifyJwt).Result;
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] string spotifyJwt)
    {
        UserDTO user = await _mediator.Send(new CreateUser(spotifyJwt));

        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userToUpdate)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
        throw new Exception("User informations are missing in the JWT.");

        UserDTO user = await _mediator.Send(new UpdateUser(userToUpdate, token.UserId));
        return Ok(user);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
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