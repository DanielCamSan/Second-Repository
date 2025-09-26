using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace FirstExam.Controllers

{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
    {
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Preto", Species = "dog", Breed = "Sharpei", Birthdate = new DateTime(2020, 5, 12), Sex = "male", WeightKg = 22.5m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Mimi", Species = "cat", Breed = "Siamese", Birthdate = new DateTime(2019, 8, 20), Sex = "female", WeightKg = 4.28m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Kiko", Species = "bird", Breed = "Parrot", Birthdate = new DateTime(2021, 1, 15), Sex = "male", WeightKg = 0.3m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Lola", Species = "dog", Breed = "Cooker", Birthdate = new DateTime(2018, 11, 5), Sex = "female", WeightKg = 28.0m },
    new Pet { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Name = "Spike", Species = "reptile", Breed = "Iguana", Birthdate = new DateTime(2022, 3, 30), Sex = "male", WeightKg = 5.1m }
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page, 
            [FromQuery] int? limit, 
            [FromQuery] string? sort, 
            [FromQuery] string? order)
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Pet> query = _pets;
            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetOne(Guid id)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == id);
            return pet is null
                ? NotFound(new { error = "Pet not found", status = 404 })
                : Ok(pet);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                OwnerId = dto.OwnerId,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                Birthdate = dto.BirthDate,
                Sex = dto.Sex.Trim(),
                WeightKg = dto.WeightKg
            };

            _pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id }, pet);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _pets.FindIndex(p => p.Id == id);
            if (index == -1)
                return NotFound(new { error = "Pet not found", status = 404 });

            var existing = _pets[index];
            var updated = new Pet
            {
                Id = id,
                OwnerId = existing.OwnerId,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                Birthdate = dto.BirthDate,
                Sex = dto.Sex.Trim(),
                WeightKg = dto.WeightKg
            };

            _pets[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _pets.RemoveAll(p => p.Id == id);
            return removed == 0
                ? NotFound(new { error = "Pet not found", status = 404 })
                : NoContent();
        }

    }
}
