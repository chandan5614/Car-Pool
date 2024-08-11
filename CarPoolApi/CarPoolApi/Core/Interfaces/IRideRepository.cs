using Core.Interfaces;
using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IRideRepository : IRepository<Ride>
    {
        Task<IEnumerable<Ride>> GetRidesByDriverAsync(Guid driverId);
        Task<IEnumerable<Ride>> SearchRidesAsync(string startLocation, string endLocation);
    }
}