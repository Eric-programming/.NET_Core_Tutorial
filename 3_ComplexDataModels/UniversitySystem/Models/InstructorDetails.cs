using System.Collections.Generic;

namespace UniversitySystem.Models
{
    public class InstructorDetails
    {
        public IEnumerable<Course> CourseTaughtByCurrentInstructor { get; set; }
        public Instructor CurrentInstructor { get; set; }
        public IEnumerable<Enrollment> AllEnrollmentFromCurrentCourse { get; set; }
    }
}