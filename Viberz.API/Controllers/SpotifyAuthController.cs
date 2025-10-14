using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Queries.Authentication;

namespace Viberz.API.Controllers
{
    public class SpotifyAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpotifyAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("getSpotifyToken")]
        public async Task<IActionResult> GetSpotifyToken([FromBody] SpotifyAuthCodeRequestDTO spotifyAuthCodeRequest)
        {
            return Ok(await _mediator.Send(new GetSpotifyTokenQuery(spotifyAuthCodeRequest)));
        }

        [HttpPost("refreshSpotifyToken")]
        public async Task<IActionResult> RefreshSpotifyToken([FromBody] RefreshSpotifyTokenDTO refreshSpotifyTokenDTO)
        {
            return Ok(await _mediator.Send(new RefreshSpotifyTokenQuery(refreshSpotifyTokenDTO)));
        }
    }
}