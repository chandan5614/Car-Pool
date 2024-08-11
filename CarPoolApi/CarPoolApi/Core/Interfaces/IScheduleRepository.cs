using Core.Interfaces;
using Entities.DTOs;

namespace Core.Interfaces
{

    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(Guid userId);
    }
}
