using CardsAndMonsters.Data.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace CardsAndMonsters.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IDuelistFactory, DuelistFactory>();


            return services;
        }
    }
}
