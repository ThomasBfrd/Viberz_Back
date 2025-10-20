using System.IdentityModel.Tokens.Jwt;
using Viberz.API.Controllers;
using Viberz.Application.Interfaces.Artists;
using Viberz.Application.Interfaces.Genres;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Queries.Artists;
using Viberz.Application.Queries.Authentication;
using Viberz.Application.Queries.Genres;
using Viberz.Application.Queries.Guess;
using Viberz.Application.Services.Artists;
using Viberz.Application.Services.Authentification;
using Viberz.Application.Services.Genres;
using Viberz.Application.Services.GuessGenre;
using Viberz.Application.Services.Spotify;
using Viberz.Application.Services.Users;
using Viberz.Application.Utilities;
using Viberz.Infrastructure.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<JwtSecurityTokenHandler>();
builder.Services.AddScoped<JwtDecode>();
builder.Services.AddScoped<ConvertImageToBase64>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<IArtistsService, ArtistsService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<IGuessService, GuessService>();

builder.Services.AddMediatR(cfg =>
{
    // Pour les controllers
    cfg.RegisterServicesFromAssembly(typeof(ArtistController).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GenresController).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GuessController).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(SpotifyAuthController).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UserController).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(XpHistoryController).Assembly);

    // Pour les Queries/Commands et leurs Handlers
    cfg.RegisterServicesFromAssembly(typeof(CreateUser).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateUser).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AddXpHistory).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetArtistsQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetSpotifyTokenQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(RefreshSpotifyTokenQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetGenresQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GuessGenreQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly);
});

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? new string[] { "www.viberz.app" };

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
