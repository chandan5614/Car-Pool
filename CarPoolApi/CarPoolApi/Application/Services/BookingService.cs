using Core.Interfaces;
using Entities.DTOs;

namespace Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            return await _bookingRepository.GetByIdAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRideAsync(Guid rideId)
        {
            return await _bookingRepository.GetBookingsByRideAsync(rideId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId)
        {
            return await _bookingRepository.GetBookingsByUserAsync(userId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
        {
            return await _bookingRepository.GetBookingsByStatusAsync(status);
        }

        public async Task<int> GetBookingCountByRideAsync(Guid rideId)
        {
            return await _bookingRepository.GetBookingCountByRideAsync(rideId);
        }

        public async Task<bool> AddBookingAsync(Booking booking)
        {
            try
            {
                await _bookingRepository.AddAsync(booking);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            try
            {
                await _bookingRepository.UpdateAsync(booking);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            try
            {
                await _bookingRepository.DeleteAsync(bookingId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
