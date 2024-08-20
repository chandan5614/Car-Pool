using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;

namespace Application.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ScheduleDto> GetScheduleByIdAsync(string scheduleId)
        {
            if (!Guid.TryParse(scheduleId, out var scheduleGuid))
            {
                throw new FormatException("The provided Schedule ID is not in a valid GUID format.");
            }

            var schedule = await _scheduleRepository.GetByIdAsync(scheduleGuid);
            if (schedule == null)
            {
                return null; // Handle null case as needed
            }

            // Convert entity to DTO and return
            return new ScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                UserId = schedule.UserId,
                Semester = schedule.Semester,
                Courses = schedule.Courses.Select(course => new CourseDto
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    ClassTime = course.ClassTime,
                    Location = course.Location
                }).ToList()
            };
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            // Convert entities to DTOs and return
            return schedules.Select(schedule => new ScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                UserId = schedule.UserId,
                Semester = schedule.Semester,
                Courses = schedule.Courses.Select(course => new CourseDto
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    ClassTime = course.ClassTime,
                    Location = course.Location
                }).ToList()
            });
        }

        public async Task AddScheduleAsync(ScheduleDto scheduleDto)
        {
            var schedule = new Schedule
            {
                ScheduleId = scheduleDto.ScheduleId,
                UserId = scheduleDto.UserId,
                Semester = scheduleDto.Semester,
                Courses = scheduleDto.Courses.Select(course => new Course
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    ClassTime = course.ClassTime,
                    Location = course.Location
                }).ToList()
            };
            await _scheduleRepository.AddAsync(schedule);
        }

        public async Task UpdateScheduleAsync(ScheduleDto scheduleDto)
        {
            var schedule = new Schedule
            {
                ScheduleId = scheduleDto.ScheduleId,
                UserId = scheduleDto.UserId,
                Semester = scheduleDto.Semester,
                Courses = scheduleDto.Courses.Select(course => new Course
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    ClassTime = course.ClassTime,
                    Location = course.Location
                }).ToList()
            };
            await _scheduleRepository.UpdateAsync(schedule);
        }

        public async Task DeleteScheduleAsync(string scheduleId)
        {
            await _scheduleRepository.DeleteAsync(Guid.Parse(scheduleId));
        }
    }
}
