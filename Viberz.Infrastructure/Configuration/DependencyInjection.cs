using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viberz.Domain.Interfaces;
using Viberz.Domain.Interfaces.Genres;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;
using Viberz.Infrastructure.Data;
using Viberz.Infrastructure.Repositories;
using Viberz.Infrastructure.Repositories.Genres;
using Viberz.Infrastructure.Repositories.Xp;

namespace Viberz.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string? connexionServer = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connexionServer);
        });

        // Repositories
        services.AddScoped<IUserXp, UserXp>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IXpHistoryRepository, XpHistoryRepository>();
        services.AddScoped<IGenresRepository, GenresRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<ILikedPlaylistsRepository, LikedPlaylistsRepository>();

        return services;
    }
}