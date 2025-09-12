using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;


namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckInsController: ControllerBase
    {
        private static readonly List<CheckIns> CheckIns = new()
        {
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "ABC123", Timestamp = DateTime.Now },
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "DEF456", Timestamp = DateTime.Now },
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "GHI789", Timestamp = DateTime.Now }
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
            if(prop == null) return src;
            if(string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase))
            {
                return src.OrderByDescending(x => prop.GetValue(x, null));
            }
            else
            {
                return src.OrderBy(x => prop.GetValue(x, null));
            }
        }


        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q,
            [FromQuery] string? name)
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<CheckIns> query = CheckIns;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => x.BadgeCode.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.BadgeCode.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var items = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new{total, page = p, limit = l, items});

        }


        [HttpGet("{id:guid}")]

        public ActionResult<CheckIns> GetOne(Guid id)
        {
            var checkIn = CheckIns.FirstOrDefault(a => a.Id == id);
            return checkIn is null
                ? NotFound(new { error = "CheckIn not found", status = 404 })
                : Ok(checkIn);
        }

        /*
        [HttpPost]

        public ActionResult<CheckIns> Create([FromBody] CreateCheckInsDto dto)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);

            var checkIn = new CheckIns
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode,
                Timestamp = dto.Timestamp
            };
            CheckIns.Add(checkIn);
            return CreatedAtAction(nameof(GetOne), new { id = checkIn.Id }, checkIn);

        }
        */


    }

}

















