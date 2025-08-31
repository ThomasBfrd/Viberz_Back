using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Viberz.Application.DTO.Artists;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Queries;
using Viberz.Application.Services;
using Viberz.Application.Utilities;

namespace Viberz.Controllers;

[Route("api/search")]
[ApiController]
public class ArtistController(GetArtistsFromSearch getArtistsFromSearch, JwtDecode jwtDecode) : ControllerBase
{
    private readonly GetArtistsFromSearch _getArtistsFromSearch = getArtistsFromSearch;
    private readonly JwtDecode _jwtDecode = jwtDecode;

    [HttpGet]
    public async Task<IActionResult> GetArtistsFromSearch([FromQuery] string artist)
    {
        UserJwtConnexion token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("Spotify access token is missing in the JWT.");

        List<ArtistSearchDTO> artists = await _getArtistsFromSearch.GetArtists(token.SpotifyJwt, artist);

        return Ok(artists);
    }
}
