using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> _checkIns = new()
        {
            new CheckIn { Id = Guid.NewGuid(), BadgedCode = "ABC123", TimesStap = DateTime.Now.AddHours(-2) },
            new CheckIn { Id = Guid.NewGuid(), BadgedCode = "XYZ789", TimesStap = DateTime.Now.AddHours(-1) },
            new CheckIn { Id = Guid.NewGuid(), BadgedCode = "LMN456", TimesStap = DateTime.Now }
        };

        // Normaliza paginación
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;

            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if (l > 100) l = 100;

            return (p, l);
        }

        // Orden dinámico por cualquier propiedad
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;

            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        // GET api/checkins?page=1&limit=5&q=ABC&sort=TimesStap&order=desc
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

            // Filtro
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.BadgedCode.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // Ordenamiento
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // GET api/checkins/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<CheckIn> GetOne(Guid id)
        {
            var checkIn = _checkIns.FirstOrDefault(c => c.Id == id);
            return checkIn is null
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : Ok(checkIn);
        }

        // POST api/checkins
        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkIn = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgedCode = dto.BadgedCode.Trim(),
                TimesStap = dto.TimesStap
            };

            _checkIns.Add(checkIn);
            return CreatedAtAction(nameof(GetOne), new { id = checkIn.Id }, checkIn);
        }

        // PUT api/checkins/{id}

        [HttpPut("{id:guid}")]
        public ActionResult<CheckIn> Update(Guid id, [FromBody] UpdateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _checkIns.FindIndex(c => c.Id == id);
            if (index == -1)
                return NotFound(new { error = "CheckIn not found", status = 404 });

            var updated = new CheckIn
            {
                Id = id,
                BadgedCode = dto.BadgedCode.Trim(),
                TimesStap = dto.TimesStap
            };

            _checkIns[index] = updated;
            return Ok(updated);
        }

        // DELETE api/checkins/{id}

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
