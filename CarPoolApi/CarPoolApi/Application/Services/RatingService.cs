using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;

namespace Application.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<RatingDto> GetRatingByIdAsync(Guid ratingId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);
            return new RatingDto
            {
                RatingId = rating.RatingId,
                RideId = rating.RideId,
                UserId = rating.UserId,
                Rating = rating.RatingValue,
                Comments = rating.Comments,
                Timestamp = rating.Timestamp
            };
        }

        public async Task<IEnumerable<RatingDto>> GetAllRatingsAsync()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return ratings.Select(r => new RatingDto
            {
                RatingId = r.RatingId,
                RideId = r.RideId,
                UserId = r.UserId,
                Rating = r.RatingValue,
                Comments = r.Comments,
                Timestamp = r.Timestamp
            });
        }

        public async Task AddRatingAsync(RatingDto rating)
        {
            var ratingEntity = new Rating
            {
                RatingId = Guid.NewGuid(),
                RideId = rating.RideId,
                UserId = rating.UserId,
                RatingValue = rating.Rating,
                Comments = rating.Comments,
                Timestamp = DateTime.UtcNow
            };

            await _ratingRepository.AddAsync(ratingEntity);
        }
    }
}
