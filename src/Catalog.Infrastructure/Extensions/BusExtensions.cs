using Catalog.Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions
{
    public static class BusExtensions
    {
        public static void AddBus(this IServiceCollection services, IConfigurationManager config)
        {
            var busSection = config.GetSection("bus");
            var user = busSection["user"];
            var password = busSection["password"];
            var host = busSection["host"];

            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("catalog", false));
                x.AddConsumer<PaymentProcessedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });


                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}