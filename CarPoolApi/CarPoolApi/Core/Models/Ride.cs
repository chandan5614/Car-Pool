public class Ride
{
    public Guid RideId { get; set; }
    public Guid DriverId { get; set; }
    public DateTime DepartureTime { get; set; }
    public Route RouteDetails { get; set; }
    public int AvailableSeats { get; set; }
    public ICollection<Booking> BookedSeats { get; set; }
    public decimal Fare { get; set; }
    public string RideCode { get; set; }
}

public class Route
{
    public string StartLocation { get; set; }
    public string EndLocation { get; set; }
    public List<string> Stops { get; set; }
}

public class Booking
{
    public Guid UserId { get; set; }
    public BookingStatus Status { get; set; }
}

public enum BookingStatus
{
    Booked,
    Cancelled
}
