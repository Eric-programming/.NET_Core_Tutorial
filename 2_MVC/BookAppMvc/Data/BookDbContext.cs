using BookAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAppMvc.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions options)
                    : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}