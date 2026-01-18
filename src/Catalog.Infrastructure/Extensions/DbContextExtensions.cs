using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        private const string FIAP_CONNECTIONSTRING_NAME = "FIAPCloudGames";

        public static void AddDataBaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(FIAP_CONNECTIONSTRING_NAME);

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(nameof(connectionString));

            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
