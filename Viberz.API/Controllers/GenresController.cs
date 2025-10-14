using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.Queries;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;
using Viberz.Viberz.Infrastructure.Data;

namespace Viberz.Viberz.API.Controllers;

[Route("api/genres")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtDecode _jwtDecode;

    public GenresController(GetSpotifyUserInformations getSpotifyUserInformations, ApplicationDbContext context, JwtDecode jwtDecode)
    {
        _context = context;
        _jwtDecode = jwtDecode;
    }

    [HttpGet("getGenres")]
    public async Task<IActionResult> GetGenres()
    {
       UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        List<string> genres = _context.Genres.Select(g => g.Name).ToList();

        if (genres.Count == 0)
        {
            return NotFound("No genres found.");
        }

        return Ok(genres);
    }
}
