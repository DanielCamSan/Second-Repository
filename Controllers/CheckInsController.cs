using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> _checkIns = new()
        {
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.UtcNow },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-54321", Timestamp = DateTime.UtcNow.AddMinutes(-30) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-99999", Timestamp = DateTime.UtcNow.AddHours(-1) }
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

            IEnumerable<CheckIn> query = _checkIns;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.BadgeCode.Contains(q, StringComparison.OrdinalIgnoreCase));
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
        [HttpGet("{id:guid}")]
        public ActionResult<CheckIn> GetOne(Guid id)
        {
            var checkIn = _checkIns.FirstOrDefault(c => c.Id == id);
            return checkIn is null
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : Ok(checkIn);
        }

        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkIn = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp
            };

            _checkIns.Add(checkIn);
            return CreatedAtAction(nameof(GetOne), new { id = checkIn.Id }, checkIn);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _checkIns.RemoveAll(c => c.Id == id);
            return removed == 0
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : NoContent();
        }
    }
}