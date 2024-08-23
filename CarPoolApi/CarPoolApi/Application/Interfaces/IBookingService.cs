using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByRideAsync(Guid rideId);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status);
        Task<int> GetBookingCountByRideAsync(Guid rideId);
        Task<bool> AddBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(Guid bookingId);
    }
}
