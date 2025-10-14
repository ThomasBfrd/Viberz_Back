using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viberz.Domain.Interfaces.Genres;
using Viberz.Infrastructure.Data;
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
            var connexionServer = configuration.GetConnectionString("DefaultConnection");
            var versionServer = ServerVersion.AutoDetect(connexionServer);
            options.UseMySql(connexionServer, versionServer);
        });

        // Repositories
        services.AddScoped<IUserXp, UserXp>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IXpHistoryRepository, XpHistoryRepository>();
        services.AddScoped<IGenresRepository, GenresRepository>();

        return services;
    }
}