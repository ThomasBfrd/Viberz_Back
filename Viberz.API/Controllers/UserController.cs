using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.User;
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
            throw new Exception("UserId is missing in the JWT.");

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
        throw new Exception("Spotify access token is missing in the JWT.");

        UserDTO user = await _mediator.Send(new UpdateUser(userToUpdate, token.UserId));
        return Ok(user);
    }
}