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
            var result = _appDbContext.Currencies.AsNoTracking().ToList();
            //var result = (from currencies in _appDbContext.Currencies
            //             select currencies).ToList();
            return Ok(result);
        }

        [HttpGet("GetAllCurrenciesAynch")]
        public async Task<IActionResult> GetAllCurrenciesAynch()
        {
            var result = await _appDbContext.Currencies.AsNoTracking().ToListAsync();
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
            //return only one record, if there are multiple records with the same name, it will return the first one
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u=>u.Title == name);
            //var result = await _appDbContext.Currencies.FindAsync(name);
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).FirstOrDefaultAsync();
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u => u.Title == name);
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).SingleAsync(); 
            //var result = await _appDbContext.Currencies.Where(u => u.Title == name).SingleOrDefaultAsync(); // Record should be unique, otherwise it will throw an exception
            //var result = await _appDbContext.Currencies.SingleOrDefaultAsync(u => u.Title == name); 


            //Fetch all the records with the same name, if there are multiple records with the same name, it will return all of them
            var result = await _appDbContext.Currencies.Where(u => u.Title == name).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{name}/{description}")]
        public async Task<IActionResult> GetCurrencyByNameandDescriptionAynch([FromRoute] string name, [FromRoute] string? description)
        {
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u => u.Title == name && u.Description == description);
            //var result = await _appDbContext.Currencies.SingleOrDefaultAsync(u => u.Title == name && u.Description == description);

            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(u =>
            //u.Title == name
            //&& (string.IsNullOrEmpty(description) || u.Description == description)
            //);

            var result = await _appDbContext.Currencies.Where(u => u.Title == name && (string.IsNullOrEmpty(description) || u.Description == description)
            ).ToListAsync();

            return Ok(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetCurrencyByIdsAync([FromBody] List<int> ids)
        {
            //var result = await _appDbContext.Currencies.Where(u => ids.Contains(u.Id)).ToListAsync();

            //Select only the required fields from the table, instead of fetching all the fields from the table
            //var result = await _appDbContext.Currencies.Where(u => ids.Contains(u.Id))
            //    .Select(u => new
            //    {
            //        u.Title,
            //        u.Description
            //    }).
            //    ToListAsync();

            var result = await (from currency in _appDbContext.Currencies
                         where ids.Contains(currency.Id)
                         select new
                         {
                             CurrencyTitle = currency.Title,
                             CurrencyDescription = currency.Description
                         }).ToListAsync();


            return Ok(result);
        }


    }
}
