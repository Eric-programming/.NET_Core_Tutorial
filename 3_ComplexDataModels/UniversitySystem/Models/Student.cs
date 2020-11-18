using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversitySystem.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //The ApplyFormatInEditMode setting allows formatting to be applied when the value is displayed in a input box for editing.
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        /**
        IEnumerable = Iterate and store each elements
        ICollection = Derive from IEnumerable add, remove, edit, getting counts, and etc.
        IList = Derive from ICollection plus insert and remove in the middle of the list
        IQuerable = derived from ICollection, it can generate LINQ to SQL expression to use in database layer 
        */
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}