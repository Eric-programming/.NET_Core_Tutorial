using System.ComponentModel.DataAnnotations;

namespace API_Advanced.Models.DTO
{
    public class BookDto
    {
        [Required]
        public string Title { get; set; }


    }
}