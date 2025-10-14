using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Viberz.Application.Queries;
using Viberz.Application.Utilities;
using Viberz.Viberz.API.Controllers;
using Viberz.Viberz.Infrastructure.Data;
using Viberz.Viberz.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<GetSpotifyUserInformations>();
builder.Services.AddHttpClient<GetArtistsFromSearch>();
builder.Services.AddHttpClient<SongFromPlaylist>();
builder.Services.AddHttpClient<SpotifyAuthService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<JwtSecurityTokenHandler>();
builder.Services.AddScoped<JwtDecode>();
builder.Services.AddScoped<ConvertImageToBase64>();
builder.Services.AddScoped<IUserXp, UserXp>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IXpHistoryRepository, XpHistoryRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserController).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(XpHistoryController).Assembly));

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() 
    ?? new string[] { "https://5k0ngk-ip-88-167-238-19.tunnelmole.net" }; // Provide default value

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connexionServer = builder.Configuration.GetConnectionString("DefaultConnection");
    var versionServer = ServerVersion.AutoDetect(connexionServer);
    options.UseMySql(connexionServer, versionServer);
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});

var app = builder.Build();

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
