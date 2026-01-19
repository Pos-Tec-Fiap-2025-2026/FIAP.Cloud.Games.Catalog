using Catalog.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Dtos
{
    public class GameInputDto
    {
        public GameInputDto() { }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public required string Description { get; set; }
        public required decimal Price { get; set; }

        public Game ToEntity()
        {
            return new Game
            {
                Name = Name,
                Description = Description,
                Price = Price,
            };
        }
    }
}
