using Catalog.Core.Dtos;
using Catalog.Core.Models;
using Catalog.Infrastructure.Persistence;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ResultBase<IEnumerable<GameDto>>> GetGamesAsync()
        {
            var result = await _repository.GetQuery<Game>()
                                            .Select(p => new GameDto
                                            {
                                                Name = p.Name,
                                                Price = p.Price,
                                                Description = p.Description,
                                                Active = p.Active,
                                            }).ToListAsync();

            return ResultBase<IEnumerable<GameDto>>.Ok(result);
        }

        public async Task<ResultBase<GameDto?>> GetGamesByIdAsync(int id)
        {
            var result = await _repository.GetGameByIdAsync(id);

            var dto = new GameDto(result);

            return ResultBase<GameDto?>.Ok(dto);
        }

        public async Task<ResultBase<GameDto>> CreateAsync(GameInputDto gameInputDto)
        {
            var game = gameInputDto.ToEntity();

            var exists = _repository.GetQuery<Game>().Any(p => p.Active && p.Name == game.Name && p.Price == game.Price);

            if (exists)
                return ResultBase<GameDto>.Failure("Jogo já cadastrado.");

            var isSuccess = await _repository.CreateAsync(game!);

            if (!isSuccess)
                return ResultBase<GameDto>.Failure("Não foi possível criar o jogo.");

            var dto = new GameDto(game);

            return ResultBase<GameDto>.Ok(dto!);
        }

        public async Task<ResultBase<GameDto>> UpdateAsync(int id, GameUpdateDto gameUpdateDto)
        {
            var game = await _repository.GetGameByIdAsync(id);

            if (game == null)
                return ResultBase<GameDto>.Failure($"Jogo com ID {id} não foi encontrado.");

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
                return ResultBase<GameDto>.Failure("Não foi possível atualizar o jogo.");

            var dto = new GameDto(game);

            return ResultBase<GameDto>.Ok(dto);
        }

        public async Task<ResultBase<GameDto>> BuyGameAsync(BuyRequest request)
        {
            var game = await _repository.GetGameByIdAsync(request.GameId);

            if (game == null)
                return ResultBase<GameDto>.Failure($"Jogo com ID {request.GameId} não foi encontrado.");

            var memberEmail = _repository.GetQuery<Member>()
                                    .Where(p => p.Id == request.UserId)
                                    .Select(p => p.Name)
                                    .FirstOrDefault();

            if (string.IsNullOrEmpty(memberEmail))
                return ResultBase<GameDto>.Failure($"Usuário com ID {request.GameId} não foi encontrado.");

            var orderEvent = new OrderPlacedEvent
            {
                Id = game.Id,
                Amount = game.Price,
                UserId = request.UserId,
                UserEmail = memberEmail,
                CreatedAt = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(orderEvent);

            var dto = new GameDto(game);

            return ResultBase<GameDto>.Ok(dto, $"Compra iniciada para o jogo {dto.Name}!");
        }

        public async Task<ResultBase<GameDto>> DisableGameAsync(int id)
        {
            var game = await _repository.GetGameByIdAsync(id);

            if (game == null)
                return ResultBase<GameDto>.Failure($"Jogo com ID {id} não foi encontrado.");

            game.Active = false;

            await _repository.UpdateAsync(game);

            var dto = new GameDto(game);

            return ResultBase<GameDto>.Ok(dto);
        }
    }

    public interface IGameServices
    {
        public Task<ResultBase<IEnumerable<GameDto>>> GetGamesAsync();
        public Task<ResultBase<GameDto?>> GetGamesByIdAsync(int id);
        public Task<ResultBase<GameDto>> CreateAsync(GameInputDto gameInputDto);
        public Task<ResultBase<GameDto>> UpdateAsync(int id, GameUpdateDto gameUpdateDto);
        public Task<ResultBase<GameDto>> BuyGameAsync(BuyRequest request);
        public Task<ResultBase<GameDto>> DisableGameAsync(int id);
    }
}
