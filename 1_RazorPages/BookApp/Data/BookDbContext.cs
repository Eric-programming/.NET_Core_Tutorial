using BookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Data
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