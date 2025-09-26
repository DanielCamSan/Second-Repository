using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
        {
            new Pet {Id = Guid.NewGuid(), Name = "Rocky", Species = "dog", Sex = "male" },
            new Pet {Id = Guid.NewGuid(), Name = "Cookie", Species = "cat", Sex = "female" },
            new Pet {Id = Guid.NewGuid(), Name = "Red", Species = "bird", Sex = "male" },
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = page.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));

        }

        //Get method

        [HttpGet]
        public ActionResult Getall(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order
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
                meta = new
                {
                    page = p,
                    limit = l,
                    total
                }
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Pet> GetOne(Guid Id)
        {
            var pet = _pets.FirstOrDefault(a => a.Id == Id);
            return pet is null
                ? NotFound(new { error = "Pet not found", status = 404 })
                : Ok(pet);
        }

        //Post method

        [HttpPost]
        public ActionResult<Pet> Create([FromBody] CreatePetDto dto)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);
            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Species = dto.Species,
                Sex = dto.Sex,
            };
            _pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id }, pet);
        }

        [HttpPut("{id : Guid}")]
        public ActionResult<Pet> Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _pets.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound(new { error = "Pet not found", status = 404 });

            var updated = new Pet
            {
                Id = id,
                Name = dto.Name,
                Species = dto.Species,
                Sex = dto.Sex,
            };

            _pets[index] = updated;
            return Ok(updated);
        }

        

    }
}

