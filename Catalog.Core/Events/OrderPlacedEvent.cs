namespace Catalog.Core.Events
{
    public record OrderPlacedEvent
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}