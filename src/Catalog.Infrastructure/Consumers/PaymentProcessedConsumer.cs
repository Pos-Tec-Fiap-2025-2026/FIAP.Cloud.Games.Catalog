using Catalog.Core.Models;
using Catalog.Infrastructure.Persistence;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Consumers
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {
        private readonly ILogger<PaymentProcessedConsumer> _logger;
        private readonly IRepository _repository;

        public PaymentProcessedConsumer(ILogger<PaymentProcessedConsumer> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var message = context.Message;

            if (message.Status == PaymentStatus.Approved)
            {
                var member = await _repository.GetQuery<Member>()
                                        .Include(p => p.Games)
                                        .Where(p => p.Id == message.UserId)
                                        .FirstOrDefaultAsync() ?? throw new Exception("Usuário não encontrado");

                var game = await _repository.GetGameByIdAsync(message.OrderId) ?? throw new Exception("Jogo não encontrado");

                member.Games.Add(game);

                _ = await _repository.UpdateAsync(member);

                _logger.LogInformation($"[SUCESSO] Jogo {message.OrderId} liberado para o usuario {message.UserId}!");
            }
            else
            {
                _logger.LogWarning($"[FALHA] Pagamento recusado para o usuario {message.Email}. Jogo nao liberado.");
            }

            await Task.CompletedTask;
        }
    }
}