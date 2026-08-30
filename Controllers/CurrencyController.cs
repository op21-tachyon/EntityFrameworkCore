using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetAllCurrencies")]
        public IActionResult GetAllCurrencies()
        {
            var result = _appDbContext.Currencies.ToList();
            //var result = (from currencies in _appDbContext.Currencies
            //             select currencies).ToList();
            return Ok(result); 
        }

        [HttpGet("GetAllCurrenciesAynch")]
        public async Task<IActionResult> GetAllCurrenciesAynch()
        {
            var result = await _appDbContext.Currencies.ToListAsync();
            //var result = (from currencies in _appDbContext.Currencies
            //             select currencies).ToList();
            return Ok(result);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAynch(int Id)
        {
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u=>u.Id == Id);
            //var result = await _appDbContext.Currencies.FindAsync(Id);
            var result = await _appDbContext.Currencies.Where(u => u.Id == Id).FirstOrDefaultAsync();
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameAynch([FromRoute] string name)
        {
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u=>u.Title == name);
            //var result = await _appDbContext.Currencies.FindAsync(name);
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).FirstOrDefaultAsync();
            var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u => u.Title == name);
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).SingleAsync(); 
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).SingleOrDefaultAsync(); // Record should be unique, otherwise it will throw an exception
            return Ok(result);
        }
    }
}
