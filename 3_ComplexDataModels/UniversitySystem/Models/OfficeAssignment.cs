using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Models
{
    public class OfficeAssignment
    {
        [Key]//Set InstrctorId as primary key
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public Instructor Instructor { get; set; }
    }
}