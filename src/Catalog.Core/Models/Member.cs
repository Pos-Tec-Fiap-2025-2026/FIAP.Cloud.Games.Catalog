namespace Catalog.Core.Models
{
    public class Member : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }

        public virtual ICollection<Game> Games { get; set; } = [];
    }
}
