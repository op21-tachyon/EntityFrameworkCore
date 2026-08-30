using EntityFrameworkCore.Data;
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
    }
}
