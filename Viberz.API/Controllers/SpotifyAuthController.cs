using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Queries.Authentication;

namespace Viberz.API.Controllers;

[Route("api/authentication")]
public class SpotifyAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpotifyAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("getSpotifyAccess")]
    public async Task<IActionResult> GetSpotifyToken([FromBody] SpotifyAuthCodeRequestDTO spotifyAuthCodeRequest)
    {
        return Ok(await _mediator.Send(new GetSpotifyTokenQuery(spotifyAuthCodeRequest)));
    }

    [HttpPost("refreshSpotifyAccess")]
    public async Task<IActionResult> RefreshSpotifyToken([FromBody] RefreshSpotifyTokenDTO refreshSpotifyTokenDTO)
    {
        return Ok(await _mediator.Send(new RefreshSpotifyTokenQuery(refreshSpotifyTokenDTO)));
    }
}