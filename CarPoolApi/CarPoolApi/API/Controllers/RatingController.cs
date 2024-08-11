using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(Guid id)
        {
            var rating = await _ratingService.GetRatingByIdAsync(id);
            if (rating == null) return NotFound();
            return Ok(rating);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatingsAsync();
            return Ok(ratings);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingDto ratingDto)
        {
            await _ratingService.AddRatingAsync(ratingDto);
            return CreatedAtAction(nameof(GetRatingById), new { id = ratingDto.RatingId }, ratingDto);
        }
    }
}
