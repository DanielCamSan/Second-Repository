using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private static readonly List<Membership> _membership = new()
        {
            new Membership{Id=Guid.NewGuid(), MemberId=Guid.NewGuid(), Plan = "Basic", StartDate=DateTime.Now,EndDate=new DateTime(2025, 10, 25, 14, 30, 0),Status="active"},
            new Membership{Id=Guid.NewGuid(),MemberId=Guid.NewGuid(), Plan= "Pro", StartDate = DateTime.Now,EndDate=new DateTime(2026, 12, 25, 14, 30, 0), Status="active"},
            new Membership{Id=Guid.NewGuid(),MemberId = Guid.NewGuid(), Plan= "Premium", StartDate =DateTime.Now,EndDate=new DateTime(2025, 12, 25, 14, 30, 0), Status="active" }
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
            [FromQuery] string? plan,
            [FromQuery] string? status,
            [FromQuery] DateTime? startdate,
            [FromQuery] DateTime? enddate
            )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Membership> query = _membership;

            if (!string.IsNullOrWhiteSpace(q))//serach (plan/status)
            {
                query = query.Where(m =>
                    m.Plan.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    m.Status.Contains(q, StringComparison.OrdinalIgnoreCase)
                );

            }
            if (!string.IsNullOrWhiteSpace(plan))
            {
                query = query.Where(m => m.Plan.Equals(plan, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(m => m.Status.Equals(plan, StringComparison.OrdinalIgnoreCase));
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




        //GET /api/v1/memberships/{id}

        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = _membership.FirstOrDefault(m => m.Id == id);
            return membership is null ? NotFound() : Ok(membership);
        }
        //POST /api/v1/memberships
        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
               
            };

            _membership.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }

        //PUT /api/v1/memberships/{id}

        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _membership.FindIndex(m => m.Id == id);
            if (index == -1)
                return NotFound(new { error = "Membershipt not found", status = 404 });

            var updated = new Membership
            {
                Id = id,
                MemberId = id,
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
            };

            _membership[index] = updated;
            return Ok(updated);
        }

        //DELETE /api/v1/memberships/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _membership.RemoveAll(m => m.Id == id);
            return removed == 0
                ? NotFound(new { error = "Membershipt not found", status = 404 })
                : NoContent();
        }

    }

}
