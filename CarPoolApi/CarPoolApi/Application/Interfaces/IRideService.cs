using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRideService
    {
        Task<RideDto> GetRideByIdAsync(Guid rideId);
        Task<IEnumerable<RideDto>> GetAllRidesAsync();
        Task AddRideAsync(RideDto ride);
        Task UpdateRideAsync(RideDto ride);
        Task DeleteRideAsync(Guid rideId);
        Task BookRideAsync(Guid rideId, Guid userId);
    }
}
