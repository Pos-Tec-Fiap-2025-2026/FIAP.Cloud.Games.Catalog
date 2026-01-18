using Catalog.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepository(this IServiceCollection services) => services.AddScoped<IRepository, Repository>();
    }
}
