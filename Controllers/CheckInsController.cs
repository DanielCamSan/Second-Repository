using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> ch = new()
        {
            new CheckIn{ id = Guid.NewGuid() , BadgeCode = "1234abcd" , Timestamp = new DateTime (2025,9,10,15,30,0) },
            new CheckIn{ id = Guid.NewGuid() , BadgeCode = "1234abcd" , Timestamp = new DateTime (2025,9,11,14,45,0) },
            new CheckIn{ id = Guid.NewGuid() , BadgeCode = "4567abcd" , Timestamp = new DateTime (2025,9,10,16,30,0) },
            new CheckIn{ id = Guid.NewGuid() , BadgeCode = "8910abcd" , Timestamp = new DateTime (2025,9,10,15,45,0) }
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

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
            IEnumerable<CheckIn> query = ch;

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
            var checkIn = ch.FirstOrDefault(c => c.id == id);
            return checkIn is null
                ? NotFound(new { error = "Checkin not found", status = 404 })
                : Ok(checkIn);
        }

     
        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckinDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkin = new CheckIn
            {
                id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp
            };
            ch.Add(checkin);
            return CreatedAtAction(nameof(GetOne), new { id = checkin.id }, ch);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = ch.RemoveAll(c => c.id == id);
            return removed == 0
                ? NotFound(new { error = "Checkin not found", status = 404 })
                : NoContent();
        }
        
    }
}










