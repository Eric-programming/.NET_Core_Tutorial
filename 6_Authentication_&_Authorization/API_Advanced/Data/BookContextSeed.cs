
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API_Advanced.Data;
using API_Advanced.Models;
using Microsoft.Extensions.Logging;

namespace API_Advanced.Data
{
    public class BookContextSeed
    {
        public static async Task SeedAsync(BookContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Books.Any())
                {
                    var booksData =
                        File.ReadAllText("Data/SeedData/books.json");

                    var books = JsonSerializer.Deserialize<List<Book>>(booksData);

                    foreach (var item in books)
                    {
                        context.Books.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<BookContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}