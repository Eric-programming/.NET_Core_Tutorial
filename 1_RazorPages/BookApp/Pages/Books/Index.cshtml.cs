using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookApp.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly BookApp.Data.BookDbContext _context;
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string BookGenre { get; set; }
        public IndexModel(BookApp.Data.BookDbContext context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; }

        public async Task OnGetAsync()
        {
            //Get Distinct Genre in list of string
            IQueryable<string> genreQuery = (from m in _context.Books
                                             orderby m.Genre
                                             select m.Genre).Distinct();
            //Get All Books
            var books = from m in _context.Books
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.ToLower().Contains(SearchString.ToLower()));
            }
            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(x => x.Genre == BookGenre);
            }
            Genres = new SelectList(await genreQuery.ToListAsync());
            Book = await books.ToListAsync();
        }
    }
}
