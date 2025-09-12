using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private static readonly List<Membership> memberships = new()
        {
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "basic",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = "active"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "pro",
                StartDate = DateTime.Now.AddMonths(-4),
                EndDate = DateTime.Now.AddMonths(-1),
                Status = "expired"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "premium",
                StartDate = DateTime.Now.AddMonths(-3),
                EndDate = DateTime.Now.AddMonths(3),
                Status = "cancelled"
            }
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

            IEnumerable<Membership> query = memberships;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Plan.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Status.Contains(q, StringComparison.OrdinalIgnoreCase));
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
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = memberships.FirstOrDefault(a => a.Id == id);
            return membership is null
                ? NotFound(new { error = "Membership not found any", status = 404 })
                : Ok(membership);
        }

        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };

            memberships.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = memberships.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Membership not found", status = 404 });

            var updated = new Membership
            {
                Id = id,
                MemberId = dto.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };

            memberships[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = memberships.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Membership not found", status = 404 })
                : NoContent();
        }

    }
}
