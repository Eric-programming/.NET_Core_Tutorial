using System.ComponentModel.DataAnnotations;

namespace BookAppMvc.Models
{
    public class Book
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string Genre { get; set; }
        [Range(1, 100)]
        public decimal Price { get; set; }
        [Required]
        public string Author { get; set; }
    }
}