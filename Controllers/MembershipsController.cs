using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GymAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private static readonly List<Membership> _memberships = new()
        {
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "premium",
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now.AddDays(330),
                Status = "active"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "basic",
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(15),
                Status = "active"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "pro",
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now.AddDays(83),
                Status = "active"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "premium",
                StartDate = DateTime.Now.AddDays(-60),
                EndDate = DateTime.Now.AddDays(305),
                Status = "expired"
            },
            new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = "basic",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(20),
                Status = "canceled"
            }
        };



        //private static readonly List<Membership> _memberships = new();

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;

            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if (l > 100) l = 100;

            return (p, l);
        }

        private static IEnumerable<Membership> OrderByProp(IEnumerable<Membership> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;

            var prop = typeof(Membership).GetProperty(sort,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

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
            [FromQuery] string? order)    
        {
            var (p, l) = NormalizePage(page, limit);

            var query = _memberships.AsEnumerable();
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new
                {
                    page = p,
                    limit = l,
                    total,
                    totalPages = (int)Math.Ceiling(total / (double)l),
                    hasNextPage = p < Math.Ceiling(total / (double)l),
                    hasPreviousPage = p > 1
                }
            });
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var membership = _memberships.FirstOrDefault(m => m.Id == id);
            if (membership == null)
            {
                return NotFound(new { error = "Membership not found", status = 404 });
            }

            return Ok(membership);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                Plan = dto.Plan.ToLower(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status.ToLower()
            };

            _memberships.Add(membership);

            return CreatedAtAction(nameof(GetById), new { id = membership.Id }, membership);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var index = _memberships.FindIndex(m => m.Id == id);
            if (index == -1)
            {
                return NotFound(new { error = "Membership not found", status = 404 });
            }

            var updated = new Membership
            {
                Id = id,
                MemberId = dto.MemberId,
                Plan = dto.Plan.ToLower(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status.ToLower()
            };

            _memberships[index] = updated;

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _memberships.RemoveAll(m => m.Id == id);
            if (removed == 0)
            {
                return NotFound(new { error = "Membership not found", status = 404 });
            }

            return NoContent();
        }
    }
}
