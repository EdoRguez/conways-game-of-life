using ConwaysGameOfLife.Core.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ConwaysGameOfLife.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<CreateBoardHandler>();
        services.AddScoped<GetNextStateHandler>();

        return services;
    }
}