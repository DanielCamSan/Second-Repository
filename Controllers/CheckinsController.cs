using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class CheckinsController : ControllerBase
    {
        private static readonly List<Checkin> checkins = new()
        {
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-678910", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-111213", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-141516", Timestamp = DateTime.Now},
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
        //Get
        [HttpGet]
        public IActionResult GetAll(
           [FromQuery] int? page,
           [FromQuery] int? limit,
           [FromQuery] string? sort,
           [FromQuery] string? order
       )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Checkin> query = checkins;
            // sorting
            query = OrderByProp(query, sort, order);

            // Pagination
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }
        // READ:
        [HttpGet("{id:guid}")]
        public ActionResult<Checkin> GetOne(Guid id)
        {
            var checkinss = checkins.FirstOrDefault(a => a.Id == id);
            return checkinss is null
                ? NotFound(new { error = "Checkin not found", status = 404 })
                : Ok(checkinss);
        }
        // POST
        [HttpPost]
        public ActionResult<Checkin> Create([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkinss = new Checkin
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
            };
            checkins.Add(checkinss);

            return CreatedAtAction(nameof(GetOne), new { id = checkinss.Id }, checkinss);
        }


    }

}