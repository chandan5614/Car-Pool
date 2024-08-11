using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRatingService
    {
        Task<RatingDto> GetRatingByIdAsync(Guid ratingId);
        Task<IEnumerable<RatingDto>> GetAllRatingsAsync();
        Task AddRatingAsync(RatingDto rating);
    }
}
