using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Dtos
{
    public class GameUpdateDto : IValidatableObject
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(0.00, double.MaxValue, ErrorMessage = "O preço não pode ser negativo.")]
        public decimal? Price { get; set; }
        public bool? Active { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isAtLeastOneFieldProvided =!string.IsNullOrEmpty(Name) ||
                                            !string.IsNullOrEmpty(Description) ||
                                            Price.HasValue ||
                                            Active.HasValue;

            if (!isAtLeastOneFieldProvided)
            {
                yield return new ValidationResult(
                    "Ao menos um campo deve ser fornecido para atualização.",
                    new[] { nameof(GameUpdateDto) }
                );
            }
        }
    }
}
