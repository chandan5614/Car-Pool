namespace Application.DTOs
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public decimal Fare { get; set; }
        public string Status { get; set; } // "pending", "completed", "failed"
        public DateTime Timestamp { get; set; }
    }
}
