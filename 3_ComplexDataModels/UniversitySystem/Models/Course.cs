using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversitySystem.Models
{
    public class Course
    {
        //The DatabaseGenerated attribute allow primary key to be created by user instead of db
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        public Department Department { get; set; }
        /**
IEnumerable = Iterate and store each elements
ICollection = Derive from IEnumerable add, remove, edit, getting counts, and etc.
IList = Derive from ICollection plus insert and remove in the middle of the list
IQuerable = derived from ICollection, it can generate LINQ to SQL expression to use in database layer 
*/
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

    }
}