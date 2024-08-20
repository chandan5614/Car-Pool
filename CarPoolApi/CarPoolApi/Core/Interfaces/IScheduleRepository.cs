using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<Schedule> GetByIdAsync(Guid scheduleId);
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(Guid userId);
    }
}
