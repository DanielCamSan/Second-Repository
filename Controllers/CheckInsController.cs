using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController: ControllerBase
    {
        private static readonly List<CheckIn> checks = new()
        {
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "123456789", Timestamp = DateTime.Now  },
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "098765432", Timestamp = DateTime.Now  },
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "24586796895678", Timestamp = DateTime.Now  },
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "245356", Timestamp = DateTime.Now  },
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "zq2wx34ec5v6tb7yn8mu9", Timestamp = DateTime.Now  },
            new CheckIn{ Id= Guid.NewGuid(), BadgeCode = "2zq3wx4ec6b7yn", Timestamp = DateTime.Now  },
        };

        private static(int pg, int lim) NormilizePg(int? pg,  int? lim)
        {
            var page = pg.GetValueOrDefault(1);
            if (page<1) page =1 ;
            var it = lim.GetValueOrDefault(10);
            if (it < 1 || it > 100) it= 1;
            return (page, it);
        }

        [HttpGet]
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
            [FromQuery] string? sort,     // ID,Badge
            [FromQuery] string? order
        )
        {
            var (p, l) = NormilizePg(page, limit);

            IEnumerable<CheckIn> query = checks;
            query = OrderByProp(query, sort, order);

            // 📄 paginación
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
            var che = checks.FirstOrDefault(a => a.Id == id);
            return che is null
                ? NotFound(new { error = "Checkin not Registered", status = 404 })
                : Ok(che);
        }

        [HttpPost]
        public ActionResult<CheckIn> Create([FromBody] CreateCheckInDTO dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var che = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode.Trim(),
                Timestamp = dto.Timestamp
            };

            checks.Add(che);
            return CreatedAtAction(nameof(GetOne), new { id = che.Id }, che);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var byebye = checks.RemoveAll(a => a.Id == id);
            return byebye == 0
                ? NotFound(new { error = "Checkin not Registered", status = 404 })
                : NoContent();
        }

    }

}
