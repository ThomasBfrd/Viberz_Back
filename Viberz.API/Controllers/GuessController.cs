using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Queries.Guess;
using Viberz.Application.Utilities;

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

    [HttpGet("guess-genre")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RandomSongsDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGuessGenreFromSpotifySong()
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        RandomSongsDTO song = await _mediator.Send(new GuessGenreQuery(token));

        return Ok(song);
    }
}
