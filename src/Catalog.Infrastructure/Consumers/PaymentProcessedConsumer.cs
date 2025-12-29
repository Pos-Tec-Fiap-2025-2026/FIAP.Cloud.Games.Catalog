using Catalog.Infrastructure.Repositories;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Consumers
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {
        private readonly InMemoryDatabase _database;
        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(InMemoryDatabase database, ILogger<PaymentProcessedConsumer> logger)
        {
            _database = database;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var message = context.Message;

            if (message.Status == PaymentStatus.Approved)
            {
                _database.AddToLibrary(message.UserId, message.OrderId);
                _logger.LogInformation($"[SUCESSO] Jogo {message.OrderId} liberado para o usuario {message.UserId}!");
            }
            else
            {
                _logger.LogWarning($"[FALHA] Pagamento recusado para o usuario {message.UserId}. Jogo nao liberado.");
            }

            return Task.CompletedTask;
        }
    }
}