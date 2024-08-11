using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatingsByRideAsync(Guid rideId);
    }
}