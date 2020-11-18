using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversitySystem.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //The ApplyFormatInEditMode setting allows formatting to be applied when the value is displayed in a input box for editing.
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        /**
        IEnumerable = Iterate and store each elements
        ICollection = Derive from IEnumerable add, remove, edit, getting counts, and etc.
        IList = Derive from ICollection plus insert and remove in the middle of the list
        IQuerable = derived from ICollection, it can generate LINQ to SQL expression to use in database layer 
        */
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}