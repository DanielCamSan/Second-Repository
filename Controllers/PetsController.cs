
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
/*
 Pets:
GET    /api/v1/pets                   (lista con paginación, orden)
GET    /api/v1/pets/{id}
POST   /api/v1/pets
PUT    /api/v1/pets/{id}
DELETE /api/v1/pets/{id}


Pet(
    Guid Id, 
    Guid OwnerId, 
    string Name, 
    string Species // dog | cat | bird | reptile | other 
    string Breed, 
    DateTime BirthDate, 
    string sex, 
    decimal? WeightKg ) 

 */
namespace newCRUD.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
        {
            new Pet {Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(),Name = "Pepe", Species = "dog", BirthDate = new DateTime(2020, 5, 1), Breed = "Golden", Sex="Femenino",WeightKg =30},
            new Pet {Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(),Name = "Juan", Species = "dog",BirthDate = new DateTime(2020, 5, 2), Breed = "Golden",Sex="Femenino",WeightKg =40},
            new Pet {Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(),Name = "Dora", Species = "dog",BirthDate = new DateTime(2020, 5, 3), Breed = "Golden",Sex="Femenino",WeightKg =50},
            new Pet {Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(),Name = "Maria", Species = "dog",BirthDate = new DateTime(2020, 5, 4), Breed = "Golden",Sex="Femenino",WeightKg =10},
            new Pet {Id = Guid.NewGuid(), OwnerId = Guid.NewGuid(),Name = "Lucho", Species = "dog",BirthDate = new DateTime(2020, 5, 5), Breed = "Golden",Sex="Femenino",WeightKg =5}
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
            [FromQuery] string? order,
            [FromQuery] string? q
        )
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
        public ActionResult<Pet> GetOne(Guid id)
        {
            var pet = _pets.FirstOrDefault(m => m.Id == id);
            return pet is null
                ? NotFound(new { error = "Pet not found", status = 404 })
                : Ok(pet);
        }

        [HttpPost]
        public ActionResult<Pet> Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                BirthDate = dto.BirthDate,
                Sex = dto.Sex.Trim(),
                WeightKg = dto.WeightKg
            };

            _pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id }, pet);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Pet> Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _pets.FindIndex(m => m.Id == id);
            if (index == -1)
                return NotFound(new { error = "Pet not found", status = 404 });

            var updated = new Pet
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                BirthDate = dto.BirthDate,
                Sex = dto.Sex.Trim(),
                WeightKg = dto.WeightKg
            };

            _pets[index] = updated;
            return Ok(updated);
        }

    }
}
  
 




