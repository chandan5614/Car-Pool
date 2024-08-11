using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;

namespace Application.Services.Implementations
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IBookingRepository _bookingRepository;

        public RideService(IRideRepository rideRepository, IBookingRepository bookingRepository)
        {
            _rideRepository = rideRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<RideDto> GetRideByIdAsync(Guid rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            return ride == null ? null : MapToDto(ride);
        }

        public async Task<IEnumerable<RideDto>> GetAllRidesAsync()
        {
            var rides = await _rideRepository.GetAllAsync();
            return rides.Select(MapToDto);
        }

        public async Task AddRideAsync(RideDto rideDto)
        {
            var ride = MapToEntity(rideDto);
            await _rideRepository.AddAsync(ride);
        }

        public async Task UpdateRideAsync(RideDto rideDto)
        {
            var ride = MapToEntity(rideDto);
            await _rideRepository.UpdateAsync(ride);
        }

        public async Task DeleteRideAsync(Guid rideId)
        {
            await _rideRepository.DeleteAsync(rideId);
        }

        public async Task BookRideAsync(Guid rideId, Guid userId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null || ride.AvailableSeats <= 0)
            {
                throw new Exception("Ride not available or full");
            }

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                RideId = rideId,
                UserId = userId,
                Status = BookingStatus.Booked,
                BookingTime = DateTime.UtcNow
            };

            ride.BookedSeats.Add(booking);
            ride.AvailableSeats--;

            await _rideRepository.UpdateAsync(ride);
            await _bookingRepository.AddAsync(booking);
        }

        private RideDto MapToDto(Ride ride)
        {
            return new RideDto
            {
                RideId = ride.RideId,
                DriverId = ride.DriverId,
                DepartureTime = ride.DepartureTime,
                Route = new RouteDto
                {
                    StartLocation = ride.RouteDetails.StartLocation,
                    EndLocation = ride.RouteDetails.EndLocation,
                    Stops = ride.RouteDetails.Stops
                },
                AvailableSeats = ride.AvailableSeats,
                BookedSeats = ride.BookedSeats.Select(b => new BookingDto
                {
                    UserId = b.UserId,
                    Status = b.Status.ToString()
                }).ToList(),
                Fare = ride.Fare,
                RideCode = ride.RideCode
            };
        }

        private Ride MapToEntity(RideDto rideDto)
        {
            return new Ride
            {
                RideId = Guid.Parse(rideDto.RideId.ToString()),
                DriverId = Guid.Parse(rideDto.DriverId.ToString()),
                DepartureTime = rideDto.DepartureTime,
                RouteDetails = new Entities.DTOs.Route
                {
                    StartLocation = rideDto.Route.StartLocation,
                    EndLocation = rideDto.Route.EndLocation,
                    Stops = (List<string>)rideDto.Route.Stops
                },
                AvailableSeats = rideDto.AvailableSeats,
                BookedSeats = rideDto.BookedSeats.Select(b => new Booking
                {
                    UserId = Guid.Parse(b.UserId.ToString()),
                    Status = Enum.Parse<BookingStatus>(b.Status)
                }).ToList(),
                Fare = rideDto.Fare,
                RideCode = rideDto.RideCode
            };
        }
    }
}
