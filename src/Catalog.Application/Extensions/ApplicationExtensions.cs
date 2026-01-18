using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddGameServices(this IServiceCollection services) => services.AddScoped<IGameServices, GameServices>();
    }
}
