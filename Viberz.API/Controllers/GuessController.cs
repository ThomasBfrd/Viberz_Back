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
    public async Task<IActionResult> GetGuessSongsList([FromQuery] List<string>? definedGenre, [FromQuery] Activies gameType)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        RandomSongsDTO song = await _mediator.Send(new GuessQuery(token, definedGenre, gameType));

        return Ok(song);
    }
}
