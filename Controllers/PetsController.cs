using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController : ControllerBase
    {
        public static readonly List<Pet> pets = new()
        {
            new Pet()
            {
                Id=Guid.NewGuid(),
                OwnerId=Guid.NewGuid(),
                Name="Perla",
                Species="cat",
                Breed="egipcio",
                BirthDate=DateTime.Now.AddMonths(-24),
                sex="macho",
                WeightKg=10
            },
            new Pet()
            {
                Id=Guid.NewGuid(),
                OwnerId=Guid.NewGuid(),
                Name="Sadu",
                Species="dog",
                Breed="Shar-Pei",
                BirthDate=DateTime.Now.AddMonths(-60),
                sex="macho",
                WeightKg=25
            },
            new Pet()
            {
                Id=Guid.NewGuid(),
                OwnerId=Guid.NewGuid(),
                Name="Fadu",
                Species="dog",
                Breed="american bully",
                BirthDate=DateTime.Now.AddMonths(-36),
                sex="macho",
                WeightKg=30
            }
        };

        public static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        public static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ? src.OrderByDescending(x => prop.GetValue(x)) : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? Page, [FromQuery] int? Limit, [FromQuery] string? Sort, [FromQuery] string? Order, [FromQuery] string? Q)
        {
            var (p, l) = NormalizePage(Page, Limit);
            IEnumerable<Pet> query = pets;
            if (!string.IsNullOrWhiteSpace(Q))
            {
                query = query.Where(a => a.Name.Contains(Q, StringComparison.OrdinalIgnoreCase) ||
                a.Breed.Contains(Q, StringComparison.OrdinalIgnoreCase) ||
                a.Species.Contains(Q, StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, Sort, Order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToArray();
            return Ok(new { data, meta = new { page = p, limit = l, total } });

        }

        [HttpGet("{id:guid}")]
        public ActionResult<Pet> GetOne(Guid id)
        {
            var pet = pets.FirstOrDefault<Pet>(pets => pets.Id == id);
            return pet is null ? NotFound(new { error = "Pet not found", status = 404 }) : Ok(pet);
        }

        [HttpPost]
        public ActionResult<Pet> Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var pet = new Pet()
            {
                Id = Guid.NewGuid(),
                OwnerId = dto.OwnerId,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                //Aqui estas asignando la especie no se crea bien
                Breed = dto.Species.Trim(),
                BirthDate = dto.BirthDate,
                sex = dto.sex.Trim(),
                WeightKg = dto.WeightKg
            };
            pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id },pet);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Pet> Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = pets.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Pet not found", status = 404 });
            var updated = new Pet()
            {
                Id = id,
                OwnerId = dto.OwnerId,
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                BirthDate = dto.BirthDate,
                sex = dto.sex.Trim(),
                WeightKg = dto.WeightKg
            };
            pets[index] = updated;
            return Ok(updated);

        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = pets.RemoveAll(pet => pet.Id == id);
            return removed == 0 ?
                NotFound(new { error = "Pet not found", status = 404 }) :
            NoContent();
        }
    }
}
