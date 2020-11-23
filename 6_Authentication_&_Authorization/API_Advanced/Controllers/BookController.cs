using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API_Advanced.Models;
using API_Advanced.Models.Interfaces;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using API_Advanced.Models.DTO;
using API_Advanced.Data;

namespace API_Advanced.Controllers
{
    [Authorize(Roles = Role.Admin + "," + Role.Editor)] //Admin or Editor
    // [Authorize(Roles = Role.Admin)]
    // [Authorize(Roles = Role.Editor)]

    public class BookController : BaseApiController
    {
        private readonly IGenericRepository<Book> _genericRepo;
        private readonly IBookRepository _bookRepository;


        public BookController(IGenericRepository<Book> genericRepo, IBookRepository bookRepository)
        {
            _genericRepo = genericRepo;
            _bookRepository = bookRepository;
        }

        // GET: api/Book
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<Book>>> GetBooks([FromQuery] BookParams bookParams)
        {
            if (bookParams != null && bookParams.Title != null)
            {
                return Ok(await _bookRepository.SearchBooks(bookParams.Title));
            }
            return Ok(await _genericRepo.ListAllAsync());
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _genericRepo.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]

        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            _genericRepo.Update(book);



            if (await _genericRepo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Fail to update book");

        }

        // POST: api/Book
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = Role.Admin)]

        public async Task<ActionResult<Book>> PostBook(BookDto bookDto)
        {
            var book = new Book()
            {
                Title = bookDto.Title
            };
            _genericRepo.Add(book);

            if (await _genericRepo.SaveAll())
            {
                return CreatedAtAction("GetBook", new { id = book.ID }, book);
            }
            throw new Exception("Fail to add book");
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]

        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _genericRepo.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _genericRepo.Delete(book);

            if (await _genericRepo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Fail to delete book");
        }
    }
}
