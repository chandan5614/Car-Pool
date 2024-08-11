using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRideService
    {
        Task<RideDto> GetRideByIdAsync(string rideId);
        Task<IEnumerable<RideDto>> GetAllRidesAsync();
        Task AddRideAsync(RideDto ride);
        Task UpdateRideAsync(RideDto ride);
        Task DeleteRideAsync(string rideId);
        Task BookRideAsync(string rideId, string userId);
    }
}
