using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using Viberz.API.Controllers;
using Viberz.Application.Factory.GuessFactory;
using Viberz.Application.Factory.PlaylistFactory;
using Viberz.Application.Interfaces.Artists;
using Viberz.Application.Interfaces.Genres;
using Viberz.Application.Interfaces.Guess;
using Viberz.Application.Interfaces.Playlist;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Application.Interfaces.User;
using Viberz.Application.Interfaces.XpHistory;
using Viberz.Application.Queries.Artists;
using Viberz.Application.Queries.Authentication;
using Viberz.Application.Queries.Genres;
using Viberz.Application.Queries.Guess;
using Viberz.Application.Services.Artists;
using Viberz.Application.Services.Authentification;
using Viberz.Application.Services.Genres;
using Viberz.Application.Services.GuessGenre;
using Viberz.Application.Services.Playlists;
using Viberz.Application.Services.Redis;
using Viberz.Application.Services.Spotify;
using Viberz.Application.Services.Users;
using Viberz.Application.Utilities;
using Viberz.Infrastructure.Configuration;
using Viberz.Infrastructure.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

if (!builder.Environment.IsDevelopment())
{
    foreach (JsonConfigurationSource source in builder.Configuration.Sources
    .ToList()) {
        source.ReloadOnChange = false;
    }
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[] {}
        }
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 5;
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<RedisService>();
builder.Services.AddTransient<JwtSecurityTokenHandler>();
builder.Services.AddScoped<ConvertImageToBase64>();
builder.Services.AddScoped<JwtDecode>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<IArtistsService, ArtistsService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<IXpHistoryService, XpHistoryService>();
builder.Services.AddScoped<IGuessService, GuessService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<GuessStrategyFactory>();
builder.Services.AddScoped<PlaylistStrategyFactory>();

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
    cfg.RegisterServicesFromAssembly(typeof(GuessQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly);
});

string[] allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? new string[] { "https://www.viberz.app" };

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});

WebApplication app = builder.Build();

await ApplyMigrationAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

static async Task ApplyMigrationAsync(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Applying database migrations...");

        bool canConnect = await context.Database.CanConnectAsync();
        if (!canConnect)
        {
            logger.LogWarning("Cannot connect to database - skipping migrations. Check database availability.");
            return;
        }

        await context.Database.MigrateAsync();

        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");

        if (app.Environment.IsProduction())
        {
            logger.LogWarning("Skipping migrations due to database error. Application will start without migrations.");
            return;
        }

        throw;
    }
}