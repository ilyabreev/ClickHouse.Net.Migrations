using Microsoft.Extensions.DependencyInjection;

namespace ClickHouse.Net.Migrations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClickHouseMigrations(this IServiceCollection services)
        {
            services.AddTransient<IClickHouseMigrations, ClickHouseMigrations>();
            services.AddTransient<IClickHouseMigrationLocator, DefaultClickHouseMigrationLocator>();
            return services;
        }
    }
}
