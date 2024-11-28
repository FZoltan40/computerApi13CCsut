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

        [HttpGet("allComputerWithOs")]
        public async Task<ActionResult<Comp>> GetAllComputerWithOs()
        {
            try
            {
                var allcomputer = await computerContext.Comps.Select(cmp => new { cmp.Brand, cmp.Type, cmp.Memory, cmp.Os.Name }).ToListAsync();

                if (allcomputer != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés.", result = allcomputer });
                }

                return NotFound(new { message = "Nincs eredmény.", result = allcomputer });
            }
            catch (Exception e)
            {

                return BadRequest(new { message = "Sikertelen lekérdezés.", result = e.Message });
            }




        }

        [HttpGet("allMicrosoftOs")]
        public async Task<ActionResult<Comp>> GetAllMicrosoftOs()
        {
            var microOs = await computerContext.Comps.Where(cmp => cmp.Os.Name.Contains("Microsoft")).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Memory, cmp.Os.Name }).ToListAsync();
            return Ok(microOs);
        }

        [HttpGet("maxDisplaySize")]
        public async Task<ActionResult<Comp>> GetMaxDiplaySize()
        {
            var maxSize = await computerContext.Comps.MaxAsync(cmp => cmp.Display);
            var maxSizeComputer = await computerContext.Comps.Where(cmp => cmp.Display == maxSize).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Display, cmp.Os.Name }).ToListAsync();
            return Ok(maxSizeComputer);
        }

        [HttpGet("newComputer")]
        public async Task<ActionResult<Comp>> GetNewComputer()
        {
            var newComputerDate = await computerContext.Comps.MaxAsync(cmp => cmp.CreatedTime);
            var newComputer = await computerContext.Comps.Where(cmp => cmp.CreatedTime == newComputerDate).Select(cmp => new { cmp, cmp.Os.Name }).ToListAsync();
            return Ok(newComputer);
        }
    }
}
