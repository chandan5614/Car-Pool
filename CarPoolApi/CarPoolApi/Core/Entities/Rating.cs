namespace Entities.DTOs
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public int RatingValue { get; set; }
        public string Comments { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
