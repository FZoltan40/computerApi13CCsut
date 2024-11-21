using ComputerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerApi.Controllers
{
    [Route("osystems")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        private readonly ComputerContext computerContext;

        public OsystemController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<Osystem>> Post(CreateOsystemDto createOsystemDto)
        {
            var os = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = createOsystemDto.Name,
                CreatedTime = DateTime.Now
            };

            if (os != null)
            {
                await computerContext.Osystems.AddAsync(os);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, os);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<Osystem>> Get()
        {
            return Ok(await computerContext.Osystems.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Osystem>> GetByID(Guid id)
        {
            var os = await computerContext.Osystems.FirstOrDefaultAsync(os => os.Id == id);

            if (os != null)
            {
                return Ok(os);
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Osystem>> Put(UpdateOsystemDto updateOsystemDto, Guid id)
        {
            var existingOs = await computerContext.Osystems.FirstOrDefaultAsync(os => os.Id == id);

            if (existingOs != null)
            {
                existingOs.Name = updateOsystemDto.Name;
                computerContext.Osystems.Update(existingOs);
                await computerContext.SaveChangesAsync();
                return Ok(existingOs);

            }
            return NotFound(new { message = "Nincs ilyen találat." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var os = await computerContext.Osystems.FirstOrDefaultAsync(o => o.Id == id);

            if (os != null)
            {
                computerContext.Osystems.Remove(os);
                await computerContext.SaveChangesAsync();
                return Ok(new { message = "Sikeres törlés!" });
            }

            return NotFound(new { message = "Nincs ilyen találat." });
        }
    }
}
