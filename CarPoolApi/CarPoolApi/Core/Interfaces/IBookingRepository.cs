using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByRideAsync(Guid rideId);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId);
    }
}
