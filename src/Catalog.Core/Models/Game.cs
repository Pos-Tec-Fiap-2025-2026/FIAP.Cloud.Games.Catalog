namespace Catalog.Core.Models
{
    public class Game : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

        public virtual ICollection<Member> Members { get; set; } = [];
    }
}