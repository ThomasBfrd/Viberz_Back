using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Artists;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Queries.Artists;
using Viberz.Application.Utilities;

namespace Viberz.API.Controllers;

[Route("api/searchArtist")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtDecode _jwtDecode;

    public ArtistController(IMediator mediator, JwtDecode jwtDecode)
    {
        _mediator = mediator;
        _jwtDecode = jwtDecode;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtistsFromSearch([FromQuery] string artist)
    {
        UserJwtConnexion token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        List<ArtistSearchDTO> artists = await _mediator.Send(new GetArtistsQuery(token.SpotifyJwt, artist));

        return Ok(artists);
    }
}
