using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRideById(Guid id)
        {
            var ride = await _rideService.GetRideByIdAsync(id);
            if (ride == null) return NotFound();
            return Ok(ride);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRides()
        {
            var rides = await _rideService.GetAllRidesAsync();
            return Ok(rides);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRide([FromBody] RideDto rideDto)
        {
            await _rideService.AddRideAsync(rideDto);
            return CreatedAtAction(nameof(GetRideById), new { id = rideDto.RideId }, rideDto);
        }
    }
}
