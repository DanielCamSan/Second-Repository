using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Xml;

namespace FirstExam.Controllers
{
    //ruta pedi api/v1 y el nombre del controller siempre en plural 
    [ApiController]
    [Route("[controller]")]
    public class OwnerController : ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner { Id = Guid.NewGuid(), Email = "JuanPerez@gmail.com", FullName = "Juan Perez", Active = true },
            new Owner { Id = Guid.NewGuid(), Email = "MariaGomez@gmail.com", FullName = "Maria Gomez", Active = true },
            new Owner { Id = Guid.NewGuid(), Email = "CarlosSanchez@gmail.com", FullName = "Carlos Sanchez", Active = false }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ?
                src.OrderByDescending(x => prop.GetValue(x, null)) :
                src.OrderBy(x => prop.GetValue(x, null));
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q,
            [FromQuery] string? filter
        )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Owner> query = _owners;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x =>
                    x.Email.ToLower().Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    x.FullName.ToLower().Contains(q, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(a => a.Equals(filter));
            }

            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new { data, meta = new { page = p, limit = l, total } });

        }

       
        [HttpGet("{id:guid}")]
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owner = _owners.FirstOrDefault(a => a.Id == id);
            return owner is null ? NotFound(new { error = "Owner not found", status = 404 }) : Ok(owner);
        }

        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };
            _owners.Add(owner);
            return CreatedAtAction(nameof(Create), new { id = owner.Id }, owner );

        }

        [HttpPut("{id:guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _owners.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Owner not found", status = 404 });
            var updated = new Owner
            {
                Id = id,
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };
            _owners[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _owners.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound(new { error = "Owner not found", status = 404 }) : NoContent();
        }


    }
}