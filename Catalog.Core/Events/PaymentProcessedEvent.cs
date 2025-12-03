namespace Catalog.Core.Events
{
    public enum PaymentStatus : byte
    {
        Approved = 0,
        Rejected = 1
    }

    public record PaymentProcessedEvent
    {
        public int UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        public PaymentStatus Status { get; init; }
        public DateTime ProcessedAt { get; init; }
    }
}