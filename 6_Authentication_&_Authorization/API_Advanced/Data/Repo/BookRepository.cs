using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Advanced.Models;
using API_Advanced.Models.Interfaces;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Advanced.Data.Repo
{
    public class BookRepository : IBookRepository
    {
        private readonly IGenericRepository<Book> _bookGenericsRepo;
        public BookRepository(IGenericRepository<Book> bookGenericsRepo)
        {
            _bookGenericsRepo = bookGenericsRepo;
        }

        public async Task<List<Book>> SearchBooks(string search)
        {
            return await _bookGenericsRepo.GetQueryable().Where(x => x.Title.ToLower().Contains(search.ToLower())).ToListAsync();
        }
    }
}