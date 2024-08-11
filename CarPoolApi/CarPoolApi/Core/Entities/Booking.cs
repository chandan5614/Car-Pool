namespace Entities.DTOs
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime BookingTime { get; set; }
    }
}