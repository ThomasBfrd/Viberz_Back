using Viberz;
using Viberz.Application.Queries;
using Viberz.Application.Services;
using Viberz.Application.Utilities;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Viberz.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<GetSpotifyUserInformations>();
builder.Services.AddHttpClient<GetArtistsFromSearch>();
builder.Services.AddHttpClient<SpotifyAuthService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<JwtSecurityTokenHandler>();
builder.Services.AddScoped<JwtDecode>();
builder.Services.AddScoped<ConvertImageToBase64>();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy => policy
    .WithOrigins(allowedOrigins)
    .WithHeaders("Authorization", "Content-Type")
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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
