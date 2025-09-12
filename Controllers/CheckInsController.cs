using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> _checkins = new()
        {
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-10001", Timestamp = DateTime.UtcNow.AddMinutes(-30) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-10002", Timestamp = DateTime.UtcNow.AddMinutes(-20) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-10003", Timestamp = DateTime.UtcNow.AddMinutes(-10) }
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


        //GET /api/v1/checkins(lista con paginación, orden)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,    
            [FromQuery] string? order    
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<CheckIn> query = _checkins;
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // GET /api/v1/checkins/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<CheckIn> GetOne(Guid id)
        {
            var item = _checkins.FirstOrDefault(c => c.Id == id);
            return item is null
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : Ok(item);
        }

        // POST /api/v1/checkins
        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkIn = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = DateTime.UtcNow
            };

            _checkins.Add(checkIn);
            return CreatedAtAction(nameof(GetOne), new { id = checkIn.Id }, checkIn);
        }

        // DELETE /api/v1/checkins/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _checkins.RemoveAll(c => c.Id == id);
            return removed == 0
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : NoContent();
        }
    }
}
