using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/pets")]
    public class PetsController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, Pets> _store = new();

        [HttpGet]
        public ActionResult<PagedResponse<Pets>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? sort = null,
            [FromQuery] string order = "asc")
        {
            
            var response = new PagedResponse<Pets>(data, new PageMeta(page, limit, total));
            return Ok(response);
        }

        // GET /api/v1/pets/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Pets> GetById(Guid id)
        {
                if (!_store.TryGetValue(id, out var pet))
                return NotFound();

            return Ok(pet);
        }

        // POST /api/v1/pets
        [HttpPost]
        public ActionResult<Pets> Create([FromBody] createPetsDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var pet = new Pets
            {
                Id        = Guid.NewGuid(),
                OwnerId   = dto.OwnerId,
                Name      = dto.Name,
                Species   = dto.Species,
                Breed     = dto.Breed,
                BirthDate = dto.BirthDate,
                Sex       = dto.sex,        
                WeightKg  = dto.WeightKg
            };

            _store[pet.Id] = pet;

            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }

        // PUT /api/v1/pets/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Pets> Update(Guid id, [FromBody] updatePetsDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!_store.TryGetValue(id, out var existing))
                return NotFound();

            existing.Name      = dto.Name;
            existing.Species   = dto.Species;
            existing.Breed     = dto.Breed;
            existing.BirthDate = dto.BirthDate;
            existing.Sex       = dto.sex; 
            existing.WeightKg  = dto.WeightKg;

            _store[id] = existing;

            return Ok(existing);
        }

        // DELETE /api/v1/pets/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            if (!_store.TryRemove(id, out _))
                return NotFound();

            return NoContent();
        }
    }

    // Tipos de respuesta para la paginación
    public record PagedResponse<T>(IEnumerable<T> Data, PageMeta Meta);
    public record PageMeta(int Page, int Limit, int Total);
}