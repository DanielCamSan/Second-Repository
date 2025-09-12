using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckinController : ControllerBase
    {
        private static readonly List<Checkin> _checkins = new()
        {
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.Now },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-45788", Timestamp = DateTime.Now },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-67811", Timestamp = DateTime.Now }
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
            [FromQuery] string? genres  
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Checkin> query = _checkins;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a => a.BadgeCode.Contains(q, StringComparison.OrdinalIgnoreCase));
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
        public ActionResult<Checkin> GetOne(Guid id)
        {
            var check = _checkins.FirstOrDefault(a => a.Id == id);
            return check is null
                ? NotFound(new { error = "Checkin not found", status = 404 })
                : Ok(check);
        }

        [HttpPost]
        public ActionResult<Checkin> Create([FromBody] CreateCheckinDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var check = new Checkin
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp
            };

            _checkins.Add(check);
            return CreatedAtAction(nameof(GetOne), new { id = check.Id }, check);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Checkin> Update(Guid id, [FromBody] UpdateCheckinDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _checkins.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Checkin not found", status = 404 });

            var updated = new Checkin
            {
                Id = id,
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp
            };

            _checkins[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _checkins.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Checkin not found", status = 404 })
                : NoContent();
        }
    }
}
