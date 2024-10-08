﻿using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetBookingsByRideAsync(Guid rideId);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status);
        Task<int> GetBookingCountByRideAsync(Guid rideId);
    }
}
