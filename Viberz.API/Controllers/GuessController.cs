using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Queries;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;
using Viberz.Viberz.Infrastructure.Data;

namespace Viberz.Viberz.API.Controllers;

[Route("api/guess")]
[ApiController]
public class GuessController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtDecode _jwtDecode;
    private readonly SongFromPlaylist _songFromPlaylist;
    private readonly IConfiguration _configuration;

    public GuessController(GetSpotifyUserInformations getSpotifyUserInformations, ApplicationDbContext context, JwtDecode jwtDecode, SongFromPlaylist songFromPlaylist , IConfiguration configuration)
    {
        _context = context;
        _jwtDecode = jwtDecode;
        _songFromPlaylist = songFromPlaylist;
        _configuration = configuration;
    }

    [HttpGet("guess-genre")]
    public async Task<IActionResult> GetGuessGenreFromSpotifySong()
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == token.UserId) ??
            throw new Exception("User not found in database");

        if (existingUser is null) return BadRequest();

        List<Genre> genres = await _context.Genres.ToListAsync() ?? 
            throw new Exception("Can't get genres");

        if (genres is null)
        {
            throw new Exception("No genres found in configuration");
        }

        Random random = new();

        int randomIndex = random.Next(genres.Count);
        string randomPlaylistId = genres[randomIndex].SpotifyId;
        string randomGenre = genres[randomIndex].Name;
        List<string> randomOtherGenres = genres
            .Select(a => a.Name)
            .Where(g => !g.Equals(randomGenre))
            .ToList();

        List<string> otherGenres = randomOtherGenres.OrderBy(x => random.Next()).Take(3).ToList();

        SongDTO songFromPlaylist = await _songFromPlaylist.GetSongFromPlaylist(token.SpotifyJwt, randomPlaylistId, randomGenre, otherGenres);

        return Ok(songFromPlaylist);
    }
}
