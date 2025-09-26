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
            // Validaciones comunes de paginación/orden
            if (page < 1) ModelState.AddModelError(nameof(page), "page debe ser >= 1");
            if (limit is < 1 or > 100) ModelState.AddModelError(nameof(limit), "limit debe estar en el rango 1–100");
            if (!string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(order), "order debe ser 'asc' o 'desc'");
            }

            // Campos de orden permitidos
            var allowedSort = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "name", "birthDate", "weightKg"
            };
            var sortField = string.IsNullOrWhiteSpace(sort) ? "name" : sort;
            if (!allowedSort.Contains(sortField))
            {
                ModelState.AddModelError(nameof(sort), $"sort debe ser uno de: {string.Join(", ", allowedSort)}");
            }

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var query = _store.Values.AsQueryable();

            // Orden
            var desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortField.ToLowerInvariant() switch
            {
                "birthdate" => (desc ? query.OrderByDescending(p => p.BirthDate) : query.OrderBy(p => p.BirthDate)),
                "weightkg"  => (desc ? query.OrderByDescending(p => p.WeightKg)  : query.OrderBy(p => p.WeightKg)),
                _           => (desc ? query.OrderByDescending(p => p.Name)      : query.OrderBy(p => p.Name)),
            };

            var total = query.Count();
            var data = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

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