using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> checkins = new()
        {
            new CheckIn { Id = Guid.NewGuid(), BadgeCode="GYM-1001", Timestamp=DateTime.UtcNow.AddHours(-5) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode="GYM-1002", Timestamp=DateTime.UtcNow.AddDays(-1) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode="GYM-1003", Timestamp=DateTime.UtcNow }
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
            [FromQuery] string? order
        )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<CheckIn> query = checkins;

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
            var checkin = checkins.FirstOrDefault(a => a.Id == id);
            return checkin is null
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : Ok(checkin);
        }
        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkin = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = DateTime.UtcNow 
            };

            checkins.Add(checkin);
            return CreatedAtAction(nameof(GetOne), new { id = checkin.Id }, checkins);

        }
        [HttpPut("{id:guid}")]

        public ActionResult<CheckIn> Update(Guid id, [FromBody] UpdateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = checkins.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "CheckIn not found", status = 404 });

            var updated = new CheckIn
            {
                Id = id,
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp.ToUniversalTime()
            };

            checkins[index] = updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = checkins.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "checkins not found", status = 404 })
                : NoContent();
        }
        
    }
}
/*
GET / api / v1 / checkins(lista con paginación, orden)

GET / api / v1 / checkins /{ id}

POST / api / v1 / checkins

DELETE / api / v1 / checkins /{ id}*/