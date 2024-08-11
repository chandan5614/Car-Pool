namespace Application.DTOs
{
    public class ScheduleDto
    {
        public Guid ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public string Semester { get; set; }
        public IEnumerable<CourseDto> Courses { get; set; }
    }

    public class CourseDto
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime ClassTime { get; set; }
        public string Location { get; set; }
    }
}
