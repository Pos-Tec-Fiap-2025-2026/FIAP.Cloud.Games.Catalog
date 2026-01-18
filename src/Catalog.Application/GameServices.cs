using Catalog.Core.Dtos;
using Catalog.Core.Models;
using Catalog.Infrastructure.Persistence;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;

namespace Catalog.Application
{
    internal class GameServices : IGameServices
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository _repository;

        public GameServices(IRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ResultBase<IEnumerable<Game>>> GetGamesAsync() => ResultBase<IEnumerable<Game>>.Ok(await _repository.GetGamesAsync());

        public async Task<ResultBase<Game?>> GetGamesByIdAsync(int id) => ResultBase<Game?>.Ok(await _repository.GetGameByIdAsync(id));

        public async Task<ResultBase<Game>> CreateAsync(GameInputDto gameInputDto)
        {
            var game = gameInputDto.ToEntity();

            var exists = _repository.GetQuery<Game>().Any(p => p.Active && p.Name == game.Name && p.Price == game.Price);

            if (exists)
                return ResultBase<Game>.Failure("Jogo já cadastrado.");

            var isSuccess = await _repository.CreateAsync(game!);

            if (!isSuccess)
                return ResultBase<Game>.Failure("Não foi possível criar o jogo.");

            return ResultBase<Game>.Ok(game!);
        }

        public async Task<ResultBase<Game>> UpdateAsync(int id, GameUpdateDto gameUpdateDto)
        {
            var game = await _repository.GetGameByIdAsync(id);

            if (game == null)
                return ResultBase<Game>.Failure($"Jogo com ID {id} não foi encontrado.");

            if (!string.IsNullOrEmpty(gameUpdateDto.Name))
                game.Name = gameUpdateDto.Name;

            if (!string.IsNullOrEmpty(gameUpdateDto.Description))
                game.Description = gameUpdateDto.Description;

            if (gameUpdateDto.Active.HasValue)
                game.Active = gameUpdateDto.Active.Value;

            if (gameUpdateDto.Price.HasValue)
                game.Price = gameUpdateDto.Price.Value;

            var isSuccess = await _repository.UpdateAsync(game);

            if (!isSuccess)
                return ResultBase<Game>.Failure("Não foi possível atualizar o jogo.");

            return ResultBase<Game>.Ok(game);
        }

        public async Task<ResultBase<Game>> BuyGameAsync(BuyRequest request)
        {
            var game = await _repository.GetGameByIdAsync(request.GameId);

            if (game == null)
                return ResultBase<Game>.Failure($"Jogo com ID {request.GameId} não foi encontrado.");

            var memberEmail = _repository.GetQuery<Member>()
                                    .Where(p => p.Id == request.UserId)
                                    .Select(p => p.Name)
                                    .FirstOrDefault();

            if (string.IsNullOrEmpty(memberEmail))
                return ResultBase<Game>.Failure($"Usuário com ID {request.GameId} não foi encontrado.");

            var orderEvent = new OrderPlacedEvent
            {
                Id = game.Id,
                Amount = game.Price,
                UserId = request.UserId,
                UserEmail = memberEmail,
                CreatedAt = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(orderEvent);

            return ResultBase<Game>.Ok(game, $"Compra iniciada para o jogo {game.Name}!");
        }

        public async Task<ResultBase<bool>> DisableGameAsync(int id)
        {
            var game = await _repository.GetGameByIdAsync(id);

            if (game == null)
                return ResultBase<bool>.Failure($"Jogo com ID {id} não foi encontrado.");

            game.Active = false;
            game.UpdatedAt = DateTime.Now;

            return ResultBase<bool>.Ok(true);
        }
    }

    public interface IGameServices
    {
        public Task<ResultBase<IEnumerable<Game>>> GetGamesAsync();
        public Task<ResultBase<Game?>> GetGamesByIdAsync(int id);
        public Task<ResultBase<Game>> CreateAsync(GameInputDto gameInputDto);
        public Task<ResultBase<Game>> UpdateAsync(int id, GameUpdateDto gameUpdateDto);
        public Task<ResultBase<Game>> BuyGameAsync(BuyRequest request);
        public Task<ResultBase<bool>> DisableGameAsync(int id);
    }
}
