using System.Collections.Generic;
using System.Threading.Tasks;
using API_Advanced.Models;

namespace Core.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> SearchBooks(string search);
    }
}