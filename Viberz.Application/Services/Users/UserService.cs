using AutoMapper;
using Viberz.Application.DTO.User;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;
using Viberz.Domain.Models;

namespace Viberz.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISpotifyService _spotifyService;
    private readonly IUserXp _userXp;
    private readonly IMapper _mapper;
    private readonly ConvertImageToBase64 _imageConverter;

    public UserService(IUserRepository userRepository, ISpotifyService spotifyService, ConvertImageToBase64 imageConverter, IUserXp userXp, IMapper mapper)
    {
        _userRepository = userRepository;
        _spotifyService = spotifyService;
        _imageConverter = imageConverter;
        _userXp = userXp;
        _mapper = mapper;
    }

    public async Task<UserDTO> CreateUser(string jwtToken)
    {
        // 1. Récupérer les infos Spotify
        UserSpotifyInformationsDTO userInfo = await _spotifyService.GetUserSpotifyInformations(jwtToken)
            ?? throw new Exception("Erreur lors de la récupération du user");

        // 2. Convertir l'image
        string imageBase64 = string.Empty;
        if (userInfo.Images?.Count > 0)
        {
            var imageUrl = userInfo.Images.FirstOrDefault()?.Url;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageFetched = await _imageConverter.ConvertJpgUrlToWebp(imageUrl);
                imageBase64 = Convert.ToBase64String(imageFetched)
                    ?? throw new Exception("Erreur dans la conversion de la photo de profile en base64.");
            }
        }

        // 3. Vérifier le type d'utilisateur
        if (userInfo.UserProduct != "premium")
            throw new Exception("You must have a premium account to use Viberz.");

        // 4. Créer l'objet User
        User user = new()
        {
            Id = userInfo.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Image = imageBase64,
            Email = userInfo.Email,
            Username = userInfo.Username ?? string.Empty,
            UserType = "premium",
            FavoriteArtists = new List<string>(),
            FavoriteGenres = new List<string>()
        };

        // 5. Persister en base via le repository
        await _userRepository.AddUser(user);

        return new UserDTO
        {
            User = user,
            Xp = new UserXpDTO
            {
                UserId = userInfo.Id,
                CurrentXp = 0,
                Level = 1
            }
        };
    }

    public async Task<UserDTO?> GetUserById(string userId)
    {
        User? user = await _userRepository.GetUser(userId);
        UserXpInfo? userXpInfo = await _userXp.GetUserXp(userId);

        if (user is not null && userXpInfo is not null)
        {
            return new UserDTO
            {
                User = user,
                Xp = _mapper.Map<UserXpDTO>(userXpInfo)
            };
        }

        return null;
    }

    public async Task<UserDTO> UpdateUser(UserUpdateDTO userToUpdate, string userId)
    {
        User user = _mapper.Map<User>(userToUpdate);
        user.Id = userId;
        user.UpdatedAt = DateTime.UtcNow;

        User updatedUser = await _userRepository.UpdateUser(user, userId);
        UserXpInfo? userXpInfo = await _userXp.GetUserXp(userId);

        return new UserDTO
        {
            User = updatedUser,
            Xp = _mapper.Map<UserXpDTO>(userXpInfo)
        };
    }
}
