using Catalog.Core.Models;

namespace Catalog.Core.Dtos
{
    public class GameDto
    {
        public GameDto() { }
        public GameDto(Game? game)
        {
            if (game == null) return;

            Name = game.Name;
            Price = game.Price;
            Description = game.Description;
            Active = game.Active;
        }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
