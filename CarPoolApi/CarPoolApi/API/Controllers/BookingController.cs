using Core.Interfaces;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("ride/{rideId}")]
        public async Task<IActionResult> GetBookingsByRide(Guid rideId)
        {
            var bookings = await _bookingService.GetBookingsByRideAsync(rideId);
            return Ok(bookings);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUser(Guid userId)
        {
            var bookings = await _bookingService.GetBookingsByUserAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetBookingsByStatus(string status)
        {
            var bookings = await _bookingService.GetBookingsByStatusAsync(status);
            return Ok(bookings);
        }

        [HttpGet("ride/{rideId}/count")]
        public async Task<IActionResult> GetBookingCountByRide(Guid rideId)
        {
            var count = await _bookingService.GetBookingCountByRideAsync(rideId);
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _bookingService.AddBookingAsync(booking);
            if (!success)
                return StatusCode(500, "A problem happened while handling your request.");

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] Booking booking)
        {
            if (id != booking.BookingId)
                return BadRequest("Booking ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _bookingService.UpdateBookingAsync(booking);
            if (!success)
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var success = await _bookingService.DeleteBookingAsync(id);
            if (!success)
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }
    }
}
