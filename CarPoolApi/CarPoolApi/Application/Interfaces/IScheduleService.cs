using Application.DTOs;

namespace Application.Interfaces
{
    public interface IScheduleService
    {
        Task<ScheduleDto> GetScheduleByIdAsync(string scheduleId);
        Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
        Task AddScheduleAsync(ScheduleDto schedule);
        Task UpdateScheduleAsync(ScheduleDto schedule);
        Task DeleteScheduleAsync(string scheduleId);
    }
}
