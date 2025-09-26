using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> pets = new()
       {
           new Pet { Id= Guid.NewGuid(), OwnerId=Guid.NewGuid(), Name= "Pablo", Species= "dog", Breed = "golden" , Birthdate =  new DateTime(2025,02,20) , Sex = "male" },
           new Pet { Id= Guid.NewGuid(), OwnerId=Guid.NewGuid(), Name= "Majo", Species= "cat", Breed = "persian" , Birthdate =  new DateTime(2025,03,15) , Sex = "male" },
           new Pet { Id= Guid.NewGuid(), OwnerId=Guid.NewGuid(), Name= "Amira", Species= "bird", Breed = "parrot" , Birthdate =  new DateTime(2025,02,28) , Sex = "male" },
           new Pet { Id= Guid.NewGuid(), OwnerId=Guid.NewGuid(), Name= "Ari", Species= "dog", Breed = "golden" , Birthdate =  new DateTime(2025,08,30) , Sex = "male" }
       };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 10) l = 10;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order
            )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Pet> query = OrderByProp(pets, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new { data, meta = new { page = p, limit = l, total } });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Pet> GetOne(Guid id)
        {
            var pet = pets.FirstOrDefault(x => x.Id == id);
            return pet is null ? NotFound(new { error = "Pet not Found", status = 404 })
                : Ok(pet);
        }
        [HttpPost]
        public ActionResult<Pet> Create([FromBody] CreatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                OwnerId = dto.OwnerId,
                Name = dto.Name,
                Species = dto.Species,
                Breed = dto.Breed,
                Birthdate = dto.Birthdate,
                Sex = dto.Sex,
                WeightKg = dto.WeightKg
            };
            pets.Add(pet);
            return CreatedAtAction(nameof(GetOne), new { id = pet.Id }, pet);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Pet> Update(Guid id, [FromBody] UpdatePetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = pets.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound(new { error = "Pet not found", status = 404 });
            var updated = new Pet 
            {
                OwnerId = dto.OwnerId,
                Name = dto.Name,
                Species = dto.Species,
                Breed = dto.Breed,
                Birthdate = dto.Birthdate,
                Sex = dto.Sex,
                WeightKg = dto.WeightKg
            };
            pets[index] = updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = pets.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound(new { error = "Pet not found", status = 404 }) : NoContent();
        }

    }
}
