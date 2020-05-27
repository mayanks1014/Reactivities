using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reactivities.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private readonly AppDbContext _context;

        public ValuesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<Value>>> GetAllValues()
        {
            var Values = await _context.Values.ToListAsync();           
            return Ok(Values);
        }

        [HttpGet("{id}")]
        public Value GetValueById(int id)
        {
            return new Value { Id=1,Name ="Mayank" };
        }

    }
}
