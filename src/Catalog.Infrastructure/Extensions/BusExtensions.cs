using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Catalog.Infrastructure.Consumers;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions
{
    public static class BusExtensions
    {
        public static void AddBus(this IServiceCollection services, IConfigurationManager config)
        {
            var awsSection = config.GetSection("Aws");
            var region = awsSection["Region"] ?? throw new ArgumentNullException("Region");
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentProcessedConsumer>();
                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(region, h =>
                    {
                        var accessKey = awsSection["AwsAccessKeyId"] ?? throw new ArgumentNullException("AwsAccessKeyId");
                        var secret = awsSection["AwsSecretAccessKey"] ?? throw new ArgumentNullException("AwsSecretAccessKey");
                        var token = awsSection["Token"] ?? throw new ArgumentNullException("Token");

                        h.Credentials(new Amazon.Runtime.SessionAWSCredentials(accessKey, secret, token));

                        var serviceUrl = awsSection["ServiceUrl"];
                        if (!string.IsNullOrEmpty(serviceUrl))
                        {
                            h.Config(new AmazonSQSConfig { ServiceURL = serviceUrl });
                            h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = serviceUrl });
                        }
                    });
                    cfg.Message<OrderPlacedEvent>(m => m.SetEntityName("OrderPlaced"));
                    cfg.Message<PaymentProcessedEvent>(m => m.SetEntityName("PaymentProcessed"));

                    cfg.ReceiveEndpoint("Catalog-PaymentProcessed", e =>
                    {
                        e.ConfigureConsumer<PaymentProcessedConsumer>(context);
                    });
                });
            });
        }
    }
}