namespace Entities.DTOs
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public decimal Fare { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}