using System;
using BookAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
namespace BookAppMvc.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BookDbContext>>()))
            {
                // Look for any Books.
                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Harry Potter",
                        Genre = "Science Fiction",
                        Author = "UnKnown",
                        Price = 7.99M
                    },

                    new Book
                    {
                        Title = "Maze Runner",
                        Genre = "Science Fiction",
                        Author = "UnKnown",
                        Price = 8.99M
                    },

                    new Book
                    {
                        Title = "Hunger Game",
                        Genre = "Science Fiction",
                        Author = "UnKnown",
                        Price = 9.99M
                    },

                    new Book
                    {
                        Title = "Calculus III",
                        Genre = "Education",
                        Author = "UnKnown",
                        Price = 3.99M
                    }
                );
                context.SaveChanges();
            }
        }
    }
}