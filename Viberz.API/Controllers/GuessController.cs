using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Queries;
using Viberz.Application.Queries.Guess;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;
using Viberz.Infrastructure.Data;

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
    public async Task<IActionResult> GetGuessGenreFromSpotifySong()
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        SongDTO song = await _mediator.Send(new GuessGenreQuery(token));

        return Ok(song);
    }
}
