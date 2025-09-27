using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //Excelente implementacion
    public class PetsController : ControllerBase
    {
        private static readonly List<Pets> _pets = new() {
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Rodrigo",Species="dog",Breed="german", BirthData = DateTime.Now,Sex= "female",WeightKg=55},
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Adrian",Species="dog",Breed="bulldog", BirthData = DateTime.Now,Sex= "male",WeightKg=80},
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Randall",Species="dog",Breed="pudle", BirthData = DateTime.Now,Sex= "female",WeightKg=65},
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
            [FromQuery] string? q,
            [FromQuery] string? name
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Pets> query = _pets;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();

                var esNumero = int.TryParse(term, out var dur);
                var esFecha = DateTime.TryParse(term, out var dt);

                query = query.Where(a =>
                    a.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (esNumero && a.WeightKg == dur) ||
                    (esFecha && a.BirthData.Date == dt.Date)
                );
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        [HttpGet("{Id:guid}")]
        public ActionResult<Pets> GetOne(Guid id)
        {
            var pet = _pets.FirstOrDefault(a => a.Id == id);
            return pet is null
                ? NotFound(new { error = "Pet not found", status = 404 })
                : Ok(pet);
        }


        [HttpPost]
        public ActionResult<Pets> Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var pet = new Pets
            {
                Id = Guid.NewGuid(),
                OwnerId= Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species= dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                BirthData = dto.BirthData,
                Sex = dto.Sex.Trim(),
            };

            _pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id }, pet);
        }


        [HttpPut("{id:guid}")]
        public ActionResult<Pets> Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _pets.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Pets
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Species = dto.Species.Trim(),
                Breed = dto.Breed.Trim(),
                BirthData = dto.BirthData,
                Sex = dto.Sex.Trim(),
            };

            _pets[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _pets.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Pet not found", status = 404 })
                : NoContent();
        }

    };

}






   
    




