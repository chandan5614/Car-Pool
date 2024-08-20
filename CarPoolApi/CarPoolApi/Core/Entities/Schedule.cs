namespace Entities.DTOs
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }
        public string id
        {
            get => ScheduleId.ToString();
            set {}
        }
        public Guid UserId { get; set; }
        public string Semester { get; set; }
        public ICollection<Course> Courses { get; set; }
    }


    public class Course
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime ClassTime { get; set; }
        public string Location { get; set; }
    }
}