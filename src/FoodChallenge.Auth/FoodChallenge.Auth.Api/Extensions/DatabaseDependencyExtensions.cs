using FoodChallenge.Auth.Api.Data.Postgres;
using FoodChallenge.Auth.Api.Data.Postgres.Seeds;
using Microsoft.EntityFrameworkCore;

namespace FoodChallenge.Auth.Api.Extensions;

public static class DatabaseDependencyExtensions
{
    public static IServiceCollection AddEfPostgresDependency(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });

        services.AddScoped<IOpenIddictSeedService, OpenIddictSeedService>();

        return services;
    }
}
