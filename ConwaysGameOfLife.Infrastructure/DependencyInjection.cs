using ConwaysGameOfLife.Core;
using ConwaysGameOfLife.Core.Repositories;
using ConwaysGameOfLife.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConwaysGameOfLife.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddContext(config)
                .AddPersistence();

        return services;
    }

    public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlite(config["DbConnectionString"]);
        });

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGameRepository, GameRepository>();

        return services;
    }
}