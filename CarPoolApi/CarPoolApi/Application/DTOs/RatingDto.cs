namespace Application.DTOs
{
    public class RatingDto
    {
        public Guid RatingId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; } // Rating out of 5
        public string Comments { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
