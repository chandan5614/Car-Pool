public interface IScheduleRepository : IRepository<Schedule>
{
    Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(Guid userId);
}
