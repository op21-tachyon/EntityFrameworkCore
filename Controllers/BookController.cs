using EntityFrameworkCore.Data;
using EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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

        [HttpGet("allasync")]
        public async Task<IActionResult> GetBookAync()
        {
            var result = await _appDbContext.Books.Select(x => new Book
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author
            }).AsNoTracking().ToListAsync();

            //Eager loading: Fetching the related Author entity along with the Book entity in a single query
            //var result = await _appDbContext.Books
            //    .Include(x=>x.Author)
            //    //.ThenInclude(x => x.Email) 
            //    .ToListAsync();

            //Explicit loading: Fetching the related Author entity separately after retrieving the Book entity
            //var result = await _appDbContext.Books.FirstAsync();
            //await _appDbContext.Entry(result).Reference(x => x.Author).LoadAsync();


            //Lazy loading: Fetching the related Author entity automatically when accessing the navigation property
            //var result = await _appDbContext.Books.FirstAsync();

            //var result = _appDbContext.Books.FromSql($"SELECT * FROM Books").ToList();

            //calling stored procedure to get all books
            //var result = _appDbContext.Books.FromSql($"EXEC SP_GetBooks").ToList();

            //caliing stored procedure to get book by id
            //var param = new SqlParameter("@Id", 1);
            //var result = _appDbContext.Books.FromSql($"EXEC SP_GetBookById {param}").ToList();


            //Updating table recor ds using ExecuteSqlAsync method
            //var result = await _appDbContext.Database.ExecuteSqlAsync($"Update Books set AuthorId = 2 where Id = 5");

            return Ok(result);
        }

        [HttpGet("all")]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Updatebook(int id, [FromBody] Book book)
        {
            var objBook = _appDbContext.Books.FirstOrDefault(x => x.Id == id);
            if (objBook == null)
            {
                return NotFound();
            }

            objBook.Title = book.Title;
            objBook.Description = book.Description;
            objBook.isActive = book.isActive;
            objBook.CreatedOn = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("UpdatebookWithSingleQuery")]
        public async Task<IActionResult> UpdatebookWithSingleQuery([FromBody] Book book)
        {
            _appDbContext.Books.Update(book);
            await _appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("UpdatebookInBulk")]
        public async Task<IActionResult> UpdatebookInBulk()
        {
            var result = await _appDbContext.Books.Where(b => b.Title == "Harry Potter 3").ExecuteUpdateAsync(b => b
            .SetProperty(p => p.Description,p => p.Title +" " +"This is a book Description for Harry Potter 3"));

            await _appDbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookById([FromRoute] int id)
        {
            //Soft delete by setting isActive to "0" for all books
            //var result = await _appDbContext.Books.Where(x=>x.Id == id).ExecuteUpdateAsync(x=>x.SetProperty(b => b.isActive, b => "0"));

            //Hard delete by removing the book from the database
            //var book = await _appDbContext.Books.FindAsync(id);
            //if (book == null)
            //{
            //    return NotFound();
            //}
            //_appDbContext.Books.Remove(book);
            //await _appDbContext.SaveChangesAsync();


            //Single databse call to delete the book by creating a new instance of the Book class with the specified id and setting its state to Deleted
            var book = new Book { Id = id };
            _appDbContext.Entry(book).State = EntityState.Deleted;
            await _appDbContext.SaveChangesAsync();

            return Ok(book);
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBookinBulkAsync()
        {
            
            //var boooks = await _appDbContext.Books.Where(x=>x.Id>5).ToListAsync();  
            //if(boooks == null || boooks.Count == 0)
            //{
            //    return NotFound();
            //}
            //_appDbContext.Books.RemoveRange(boooks);
            //var result = await _appDbContext.SaveChangesAsync();

            //Single query to delete the book by creating a new instance of the Book class with the specified id and setting its state to Deleted
            var result = await _appDbContext.Books.Where(x => x.Id == 5).ExecuteDeleteAsync();
            return Ok(result);
        }


    }
}
