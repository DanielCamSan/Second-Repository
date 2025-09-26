using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PetsController: ControllerBase
    {
        private static readonly List<Pet> pets = new() {
            new Pet{name="Pepe",species="Dog",breed="Chihuahua",sex="M",weightKg=18},
            new Pet{name="Tony",species="Cat",breed="Bald",sex="F",weightKg=29}
        };
        private static(int page, int limit )NormalizePage(int? page, int? limit) {
            var p = page.GetValueOrDefault(1);
            var l=limit.GetValueOrDefault(10);
            if (p < 1) p = 1;
            if (l < 1) l= 1;
            if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string ? sort, string? order)
        {
            if (string.IsNullOrEmpty(sort)) return src;
            var prop=typeof(T).GetProperty(sort, BindingFlags.IgnoreCase| BindingFlags.Public| BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ? src.OrderByDescending(x => prop.GetValue(x)) : src.OrderBy(x => prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult getAll([FromQuery]int? page, [FromQuery] int?limit, [FromQuery] string? sort, [FromQuery]string?order, [FromQuery]string? q)
        {
            var (p, l) = NormalizePage(page,limit);
            IEnumerable<Pet> query = pets;
            if (!string.IsNullOrEmpty(q))
            {
                query=query.Where(x => x.name.Contains(q, StringComparison.OrdinalIgnoreCase));    
            }
            query=OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p-1)*l).Take(l).ToList();
            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Pet> getOne(Guid id)
        {
            var search = pets.FirstOrDefault(x => x.id == id);
            return search is null ? NotFound(new { error = "Pet not found", status = 404 }):search;
        }

        [HttpPost]
        public ActionResult<Pet> Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.species.ToLower() != "dog" && dto.species.ToLower() != "cat" && dto.species.ToLower() != "bird" && dto.species.ToLower() != "reptile" && dto.species.ToLower() != "other")
            {
                return BadRequest(new { error = "species not defined", status = 400 });
            }
            if (dto.sex.ToLower() != "male" && dto.sex.ToLower() != "female")
            {
                return BadRequest(new { error = "sex not correct", status = 400 });
            }
            var pet = new Pet
            {
                ownerId=dto.ownerId,
                name = dto.name,
                species=dto.species,
                breed=dto.breed,
                sex=dto.sex,
                weightKg=dto.weightKg
            };
            pets.Add(pet);
            return CreatedAtAction(nameof(getOne), new { id = pet.id },pet);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Pet> Update(Guid id,[FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = pets.FindIndex(a => a.id == id);
            if (dto.species.ToLower() != "dog" && dto.species.ToLower() != "cat" && dto.species.ToLower() != "bird" && dto.species.ToLower() != "reptile" && dto.species.ToLower() != "other")
            {
                return BadRequest(new { error = "Specie not defined", status = 400 });
            }
            if (dto.sex.ToLower() != "male" && dto.sex.ToLower() != "female")
            {
                return BadRequest(new { error = "sex not correct", status = 400 });
            }
            if (index == -1) return NotFound(new {error="Pet not found", status=404});
            var pet = new Pet
            {
                id =id,
                ownerId = dto.ownerId,
                name = dto.name,
                species = dto.species,
                breed = dto.breed,
                sex = dto.sex,
                weightKg = dto.weightKg
            };
            pets[index] = pet;
            return Ok(pet);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = pets.RemoveAll(a=>a.id==id);
            return removed == 0 ? NotFound() : NoContent();

        }
    }
}
