namespace Application.DTOs
{
    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; } // "open", "in_progress", "resolved", "closed"
        public DateTime Timestamp { get; set; }
    }
}
