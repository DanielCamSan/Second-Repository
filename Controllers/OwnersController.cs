using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner {Id=Guid.NewGuid(), Email="Juan@gmail.com",FullName = " Juan Perez", Active=true},
            new Owner {Id=Guid.NewGuid(), Email="Maria@gmail.com",FullName = " Maria De los angeles", Active=true},
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if (l > 100) l = 1;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ? src.OrderByDescending(x => prop.GetValue(x)) : src.OrderBy(x => prop.GetValue(x));
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
            IEnumerable<Owner> query = _owners;
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(o => o.Email.Contains(q, StringComparison.OrdinalIgnoreCase) || o.FullName.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new { data, meta = new { page = p, limit = l, total } });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owner = _owners.FirstOrDefault(o => o.Id == id);
            return owner is null ? NotFound(new { error = "Owner not found", status = 404 }) : Ok(owner);
        }

        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName,
                Active = true
            };
            _owners.Add(owner);
            return CreatedAtAction(nameof(GetOne), new { id = owner.Id }, owner);

        }
        [HttpPut("{id:guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _owners.FindIndex(o => o.Id == id);
            if (index == -1) return NotFound(new { error = "Ownwe not found", status = 404 });
            var updated = new Owner
            {
                Id = id,
                Email = dto.Email,
                FullName = dto.FullName,
                Active = true

            };
            _owners.Add(updated);
            return Ok(updated);


        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _owners.RemoveAll(o => o.Id == id);
            return removed == 0 ? NotFound(new { error = "Owner not found", status = 404 }) : NoContent();
        }
    }
}
