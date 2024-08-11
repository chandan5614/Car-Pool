using Entities.DTOs;

namespace Core.Interfaces
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<IEnumerable<Report>> GetReportsByUserAsync(Guid userId);
    }
}