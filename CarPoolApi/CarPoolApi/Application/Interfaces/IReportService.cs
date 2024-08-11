using Application.DTOs;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GetReportByIdAsync(string reportId);
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task AddReportAsync(ReportDto report);
        Task UpdateReportStatusAsync(string reportId, ReportStatus status);
    }
}
