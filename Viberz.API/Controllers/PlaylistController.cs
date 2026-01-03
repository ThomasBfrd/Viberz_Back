using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Viberz.Application.Commands.Playlist;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.Queries.Playlists;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;

namespace Viberz.API.Controllers;

[Route("api/playlist")]
public class PlaylistController(JwtDecode jwtDecode, IMediator mediator) : ControllerBase
{
    private readonly JwtDecode _jwtDecode = jwtDecode;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaylistDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPlaylist([FromQuery] bool? owner, [FromQuery] int? playlistId, [FromQuery] FamilyGenres? family, [FromQuery] string? search, [FromQuery] bool likes, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
           throw new Exception("User informations are missing in the JWT.");

        List<PlaylistDTO> playlists = await _mediator.Send(new GetPlaylistsQuery(token.UserId, owner, playlistId, family, search, likes, page, pageSize));

        PaginatedPlaylistResponse<PlaylistDTO> paginatedResponse = new()
        {
            AccessToken = token.SpotifyJwt,
            Items = playlists,
            TotalItems = playlists.Count,
            PageSize = pageSize ?? playlists.Count,
            CurrentPage = page ?? 1
        };

        return Ok(paginatedResponse);
    }

    [HttpPost("like")]
    public async Task<IActionResult> LikePlaylist([FromBody] int playlistId)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
           throw new Exception("User informations are missing in the JWT.");

        bool result = await _mediator.Send(new LikePlaylist(playlistId, token.UserId));

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlaylist([FromBody] AddPlaylistDTO newPlaylist)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
           throw new Exception("User informations are missing in the JWT.");

        PlaylistDTO result = await _mediator.Send(new AddPlaylist(newPlaylist, token.UserId));

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePlaylist([FromBody] UpdatePlaylistDTO playlistToUpdate)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        PlaylistDTO result = await _mediator.Send(new UpdatePlaylist(playlistToUpdate, token.UserId));

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePlaylist([FromQuery] int playlistId)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("User informations are missing in the JWT.");

        bool result = await _mediator.Send(new DeletePlaylist(playlistId, token.UserId));

        return Ok(result);
    }
}
