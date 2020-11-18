using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }//foreign key property 
        public int StudentID { get; set; }//foreign key property 
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }//navigation property
        public Student Student { get; set; }//navigation property
    }
}