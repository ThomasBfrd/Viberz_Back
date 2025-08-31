using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.User;
using Viberz.Application.Queries;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;

namespace Viberz.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(HttpClient httpClient, GetSpotifyUserInformations getSpotifyUserInformations, ApplicationDbContext context, JwtDecode jwtDecode, ConvertImageToBase64 convertImageToBase64, IMapper mapper) : ControllerBase
{
    private readonly GetSpotifyUserInformations _getSpotifyUserInformations = getSpotifyUserInformations;
    private readonly ApplicationDbContext _context = context;
    private readonly JwtDecode _jwtDecode = jwtDecode;
    private readonly ConvertImageToBase64 _convertImageToBase64 = convertImageToBase64;
    private readonly IMapper _mapper = mapper;

    [HttpGet("me")]
    public async Task<IActionResult> GetUsers()
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        if (_context.Users.Any(u => u.Id == token.UserId))
        {
            User existingUser = _context.Users.FirstOrDefault(u => u.Id == token.UserId) ??
                throw new Exception("Souci lors de la récupération du user");

            return Ok(existingUser);
        }
        else
        {
            UserSpotifyInformationsDTO user = await _getSpotifyUserInformations.getUserSpotifyInformations(token.SpotifyJwt) ??
                throw new Exception("erreur lors de la récupération du user");

            string imageBase64 = string.Empty;

            if (user.Images?.Count > 0)
            {
                var imageUrl = user.Images.FirstOrDefault()?.Url;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    byte[] imageFetched = await _convertImageToBase64.ConvertJpgUrlToWebp(imageUrl);
                    imageBase64 = Convert.ToBase64String(imageFetched) ??
                        throw new Exception("Erreur dans la conversion de la photo de profile en base64.");
                }
            }

            User newUser = new()
            {
                Id = user.Id,
                Username = null,
                Email = user.Email,
                Image = imageBase64,
                UserType = user.UserType ?? "premium",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FavoriteArtists = new List<string>(),
                FavoriteGenres = new List<string>()
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(newUser);
        }
    }
    [HttpPatch("me")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userToUpdate)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("Spotify access token is missing in the JWT.");

        if (token.UserId is not null && _context.Users.Any(u => u.Id == token.UserId))
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == token.UserId) ??
                throw new Exception("Souci lors de la récupération du user");

            User updatedUser = _mapper.Map(userToUpdate, existingUser);

            updatedUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(updatedUser);
            await _context.SaveChangesAsync();

            return Ok(updatedUser);
        }

        return NotFound("User not found.");
    }

}