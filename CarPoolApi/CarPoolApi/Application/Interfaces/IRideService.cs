using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
