using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;


namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private static readonly List<Membership> _memberships = new()
        {
            new Membership{id=Guid.NewGuid(), MemberId = Guid.NewGuid(), Endtime = DateTime.Now, StartTime =DateTime.Now, plan = "Pro", status = "Active"},
            new Membership{id=Guid.NewGuid(), MemberId = Guid.NewGuid(), Endtime = DateTime.Now, StartTime =DateTime.Now, plan = "Pro", status = "Active"},
            new Membership{id=Guid.NewGuid(), MemberId = Guid.NewGuid(), Endtime = DateTime.Now, StartTime =DateTime.Now, plan = "Pro", status = "Active"},
            new Membership{id=Guid.NewGuid(), MemberId = Guid.NewGuid(), Endtime = DateTime.Now, StartTime =DateTime.Now, plan = "Pro", status = "Active"}
        };

        [HttpGet]
        public IActionResult GetAll(
          [FromQuery] int? page,
          [FromQuery] int? limit,
          [FromQuery] string? sort,
          [FromQuery] string? order,
          [FromQuery] string? q,
          [FromQuery] string? plan
      )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Membership> query = _memberships;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();

                var esFecha = DateTime.TryParse(term, out var dt);

                query = query.Where(a =>
                    a.plan.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    a.status.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (esFecha && a.Endtime == dt.Date) ||
                    (esFecha && a.StartTime == dt.Date)
                );
            }

            if (!string.IsNullOrWhiteSpace(plan))
            {
                query = query.Where(a => a.plan.Equals(plan, StringComparison.OrdinalIgnoreCase));
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

        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = _memberships.FirstOrDefault(a => a.id == id);
            return membership is null
                ? NotFound(new { error = "Membership not found", status = 404 })
                : Ok(membership);
        }

        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var membership = new Membership
            {
                id = Guid.NewGuid(),
                MemberId= Guid.NewGuid(),
                status =  dto.status.Trim(),
                plan = dto.plan.Trim(),
                StartTime = DateTime.Now,
                Endtime = DateTime.Now,
             
            };

            _memberships.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.id }, membership);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _memberships.FindIndex(a => a.id == id);
            if (index == -1)
                return NotFound(new { error = "Membership not found", status = 404 });

            var updated = new Membership
            {
                id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                status = dto.status.Trim(),
                plan = dto.plan.Trim(),
                StartTime = DateTime.Now,
                Endtime = DateTime.Now,
            };

            _memberships[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _memberships.RemoveAll(a => a.id == id);
            return removed == 0
                ? NotFound(new { error = "Membership not found", status = 404 })
                : NoContent();
        }
    }
}
