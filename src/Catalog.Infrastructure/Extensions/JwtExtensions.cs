using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Infrastructure.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
                                                                    WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            var jwtName = nameof(JwtSettings);
            var section = config.GetSection(jwtName) ?? throw new ArgumentException(nameof(jwtName));
            var jwtSettings = section.Get<JwtSettings>() ?? throw new ArgumentException(nameof(jwtName));

            if (string.IsNullOrEmpty(jwtSettings.Secret))
                throw new InvalidOperationException("Configurações JWT (JwtSettings:Secret) não encontradas ou inválidas.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

                options.RequireHttpsMetadata = builder.Environment.IsProduction();
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization();

            services.Configure<JwtSettings>(section);
            return services;
        }
    }
}
