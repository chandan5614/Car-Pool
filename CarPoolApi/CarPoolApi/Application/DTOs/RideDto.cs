namespace Application.DTOs
{
    public class RideDto
    {
        public Guid RideId { get; set; }
        public Guid DriverId { get; set; }
        public DateTime DepartureTime { get; set; }
        public RouteDto Route { get; set; }
        public int AvailableSeats { get; set; }
        public IEnumerable<BookingDto> BookedSeats { get; set; }
        public decimal Fare { get; set; }
        public string RideCode { get; set; }
    }

    public class RouteDto
    {
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public IEnumerable<string> Stops { get; set; }
    }

    public class BookingDto
    {
        public Guid UserId { get; set; }
        public string Status { get; set; } // "booked", "cancelled"
    }
}
