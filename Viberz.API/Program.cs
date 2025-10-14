using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
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

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

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

// Remplacer les multiples appels AddMediatR par un seul
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
    ?? new string[] { "https://5k0ngk-ip-88-167-238-19.tunnelmole.net" }; // Provide default value

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

// Probe ciblé des assemblies principaux AVANT le build/app.MapControllers
ProbeAssemblies(new[]
{
    typeof(UserController).Assembly,                    // Viberz.API
    typeof(AutoMapperProfiles).Assembly,                // Viberz.Application
    typeof(DependencyInjection).Assembly                // Viberz.Infrastructure
});

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

// Appel protégé + log complet des LoaderExceptions
try
{
    app.MapControllers();
}
catch (ReflectionTypeLoadException ex)
{
    DumpReflectionTypeLoadException("MapControllers", ex);
    throw;
}

app.Run();

// ===== Helpers locaux =====
static void ProbeAssemblies(IEnumerable<Assembly> assemblies)
{
    foreach (var asm in assemblies)
    {
        try
        {
            _ = asm.GetTypes();
            Console.WriteLine($"[Probe OK] {asm.FullName}");
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.WriteLine($"[Probe FAIL] {asm.FullName}");
            DumpReflectionTypeLoadException($"Probe {asm.GetName().Name}", ex);
            throw;
        }
    }
}

static void DumpReflectionTypeLoadException(string context, ReflectionTypeLoadException ex)
{
    Console.WriteLine($"=== ReflectionTypeLoadException during {context} ===");
    if (ex.Types is not null)
    {
        foreach (var t in ex.Types)
        {
            if (t is null) continue;
            Console.WriteLine($"Type: {t.FullName} from {t.Assembly.FullName}");
        }
    }
    if (ex.LoaderExceptions is not null)
    {
        foreach (var le in ex.LoaderExceptions)
        {
            Console.WriteLine($"Loader: {le.GetType().Name} - {le.Message}");
            if (le is System.IO.FileNotFoundException fnf)
            {
                Console.WriteLine($"Missing: {fnf.FileName}");
                if (!string.IsNullOrWhiteSpace(fnf.FusionLog))
                {
                    Console.WriteLine("FusionLog:");
                    Console.WriteLine(fnf.FusionLog);
                }
            }
        }
    }
}