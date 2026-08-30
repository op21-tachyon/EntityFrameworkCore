using EntityFrameworkCore.Data;
using EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BookController(AppDbContext appContext)
        {
            _appDbContext = appContext;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var result = (from books in _appDbContext.Books
                          select books).ToList();
            return Ok(result);
        }

        [HttpPost("Addbook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book)
        {
            book.CreatedOn = DateTime.Now;
            _appDbContext.Books.Add(book);
            await _appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPost("books")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> books)
        {
            foreach (var objbook in books)
            {
                objbook.CreatedOn = DateTime.Now;
            }
            _appDbContext.Books.AddRange(books);
            await _appDbContext.SaveChangesAsync();
            return Ok(books);
        }


    }
}
