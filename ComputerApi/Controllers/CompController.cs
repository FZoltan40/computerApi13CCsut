using ComputerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerApi.Controllers
{
    [Route("computers")]
    [ApiController]
    public class CompController : ControllerBase
    {
        private readonly ComputerContext computerContext;

        public CompController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<Comp>> Post(CreateComputerDto createComputerDto)
        {
            var cmp = new Comp
            {
                Id = Guid.NewGuid(),
                Brand = createComputerDto.Brand,
                Type = createComputerDto.Type,
                Display = createComputerDto.Display,
                Memory = createComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = createComputerDto.OsId
            };

            if (cmp != null)
            {
                await computerContext.Comps.AddAsync(cmp);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, cmp);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllCompByID(Guid id)
        {
            var os = computerContext.Osystems.Include(os => os.Comps).Where(c => c.Id == id);

            if (os != null)
            {
                return Ok(os);
            }
            return BadRequest();
        }

        [HttpGet("numOfComps/{id}")]
        public async Task<ActionResult> GetNumberOfComp(Guid id)
        {
            var numOfComps = computerContext.Comps.Where(c => c.OsId == id).Count();

            if (numOfComps != null)
            {
                return Ok(new { message = $"Az adott os-hez {numOfComps} szamgép tartozik." });
            }
            return BadRequest(new { message = "Nincs találat." });
        }
    }
}
