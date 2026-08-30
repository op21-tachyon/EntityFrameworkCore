using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public CurrencyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetAllCurrencies()
        {
            //var currencies = _appDbContext.Currencies.ToList();
            var result = (from currencies in _appDbContext.Currencies
                         select currencies).ToList();
            return Ok(result); 
        }
    }
}
