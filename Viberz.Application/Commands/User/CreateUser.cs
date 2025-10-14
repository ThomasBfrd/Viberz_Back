using MediatR;
using Viberz.Application.DTO.User;
using Viberz.Application.Queries;
using Viberz.Application.Utilities;
using Viberz.Domain.Entities;

public class CreateUser : IRequest<User>
{
    public string AccessToken { get; }

    public CreateUser(string accessToken)
    {
        AccessToken = accessToken;
    }

    // Le Handler interne gère toute la logique métier
    public class Handler : IRequestHandler<CreateUser, User>
    {
        // Injection des dépendances nécessaires
        private readonly IUserRepository _userRepository;
        private readonly GetSpotifyUserInformations _getSpotifyUserInformations;
        private readonly ConvertImageToBase64 _convertImageToBase64;

        public Handler(
            IUserRepository userRepository,
            GetSpotifyUserInformations getSpotifyUserInformations,
            ConvertImageToBase64 convertImageToBase64)
        {
            _userRepository = userRepository;
            _getSpotifyUserInformations = getSpotifyUserInformations;
            _convertImageToBase64 = convertImageToBase64;
        }

        public async Task<User> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            // 1. Récupérer les infos Spotify
            UserSpotifyInformationsDTO userInfo = await _getSpotifyUserInformations.getUserSpotifyInformations(request.AccessToken)
                ?? throw new Exception("Erreur lors de la récupération du user");

            // 2. Convertir l'image
            string imageBase64 = string.Empty;
            if (userInfo.Images?.Count > 0)
            {
                var imageUrl = userInfo.Images.FirstOrDefault()?.Url;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var imageFetched = await _convertImageToBase64.ConvertJpgUrlToWebp(imageUrl);
                    imageBase64 = Convert.ToBase64String(imageFetched)
                        ?? throw new Exception("Erreur dans la conversion de la photo de profile en base64.");
                }
            }

            // 3. Vérifier le type d'utilisateur
            if (userInfo.UserProduct != "premium")
                throw new Exception("You must have a premium account to use Viberz.");

            // 4. Créer l'objet User
            User newUser = new()
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
            return await _userRepository.AddUser(newUser);
        }
    }
}
