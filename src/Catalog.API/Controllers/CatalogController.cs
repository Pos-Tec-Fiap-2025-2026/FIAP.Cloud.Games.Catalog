using Catalog.Infrastructure.Repositories;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly InMemoryDatabase _database;
        private readonly IPublishEndpoint _publishEndpoint;

        public CatalogController(InMemoryDatabase database, IPublishEndpoint publishEndpoint)
        {
            _database = database;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("games")]
        public IActionResult GetGames() => Ok(_database.Games);

        [HttpGet("library/{userId}")]
        public IActionResult GetLibrary(int userId)
        {
            if(_database.UserLibraries.TryGetValue(userId, out var games))
                return Ok(games);
            
            return Ok(new List<Guid>());
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyGame([FromBody] BuyRequest request)
        {
            var game = _database.Games.FirstOrDefault(g => g.Id == request.GameId);
            if (game == null) return NotFound("Jogo nao encontrado");
            var orderEvent = new OrderPlacedEvent
            {
                Id = game.Id, 
                Amount = game.Price,
                UserId = request.UserId,
                UserEmail = "user@test.com",
                CreatedAt = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(orderEvent);

            return Accepted(new { message = "Compra iniciada!", orderId = orderEvent.Id });
        }
    }

    public record BuyRequest(int UserId, int GameId);
}