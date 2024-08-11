namespace Entities.DTOs
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public string Issue { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}