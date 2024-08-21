<<<<<<< HEAD
﻿using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<Schedule> GetByIdAsync(Guid scheduleId);
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Guid id);
=======
﻿using Core.Interfaces;
using Entities.DTOs;

namespace Core.Interfaces
{

    public interface IScheduleRepository : IRepository<Schedule>
    {
>>>>>>> origin/dev
        Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(Guid userId);
    }
}
