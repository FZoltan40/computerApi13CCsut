using ComputerApi.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
