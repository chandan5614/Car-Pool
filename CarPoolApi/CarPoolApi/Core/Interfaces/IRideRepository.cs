public interface IRideRepository : IRepository<Ride>
{
    Task<IEnumerable<Ride>> GetRidesByDriverAsync(Guid driverId);
    Task<IEnumerable<Ride>> SearchRidesAsync(string startLocation, string endLocation);
}
