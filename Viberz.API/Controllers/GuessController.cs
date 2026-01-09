using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Queries.Guess;
using Viberz.Application.Utilities;
using Viberz.Domain.Enums;

namespace Viberz.API.Controllers;

[Route("api/guess")]
[ApiController]
public class GuessController : ControllerBase
{
    private readonly JwtDecode _jwtDecode;
    private readonly IMediator _mediator;

    public GuessController(JwtDecode jwtDecode, IMediator mediator)
    {
        _jwtDecode = jwtDecode;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RandomSongsDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGuessSongsList([FromQuery] List<string>? definedGenre, [FromQuery] Profile profile, [FromQuery] Activies gameType)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        if (token.UserId.Length == 0)
        {
            string jwt = Request.Headers.Authorization.ToString();
            token.UserId = jwt.Split('.')[2].Substring(0, Math.Min(10, jwt.Length));
        }

        RandomSongsDTO song = await _mediator.Send(new GuessQuery(token, definedGenre, profile, gameType));

        return Ok(song);
    }
}
