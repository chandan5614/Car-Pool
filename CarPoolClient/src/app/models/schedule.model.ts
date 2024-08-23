export interface ScheduleDto {
  scheduleId: string;
  userId: string;
  semester?: string;
  courses?: CourseDto[];
}

export interface CourseDto {
  courseId?: string;
  courseName?: string;
  classTime?: string;
  location?: string;
}
