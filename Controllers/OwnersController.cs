using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
namespace Owners.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class OwnersController : ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner {Id=Guid.NewGuid(), Email="john@example.com", FullName="John Smith", Active=true}
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 10; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderBy(x => prop.GetValue(x))
                : src.OrderByDescending(x => prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? limit, [FromQuery] string? sort, [FromQuery] string? order)
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Owner> query = OrderByProp(_owners, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new { data, meta = new { page = p, limit = l, total } });

        }
        [HttpGet("{id:Guid}")]
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owners = _owners.FirstOrDefault(a => a.Id == id);
            return owners is null ? NotFound(new { error = "Owner not found", status = 404 }) : Ok(owners);
        }
        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var owners = new Owner { Id = Guid.NewGuid(), Email = dto.Email, FullName = dto.FullName, Active = dto.Active };
            _owners.Add(owners);
            return CreatedAtAction(nameof(GetOne), new { id = owners.Id }, owners);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _owners.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound(new { error = "Owner not found", status = 404 });
            var updated = new Owner { Id = Guid.NewGuid(), Email = dto.Email, FullName = dto.FullName, Active = dto.Active };
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