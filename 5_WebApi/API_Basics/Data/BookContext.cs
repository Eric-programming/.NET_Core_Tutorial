using API_Basics.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Basics.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}