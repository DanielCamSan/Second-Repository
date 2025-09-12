using System.Reflection;
using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController:ControllerBase
    {
        private static readonly List<Membership> memberships = new() {
            new Membership{MemberId=Guid.NewGuid(),Plan="basic",Status="active"},
            new Membership{MemberId=Guid.NewGuid(),Plan="pro",Status="active"},
            new Membership{MemberId=Guid.NewGuid(),Plan="pro",Status="active"},
            new Membership{MemberId=Guid.NewGuid(),Plan="premium",Status="active"}
        };
        private static(int page,int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1);
            if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10);
            if (l < 1) l = 1;
            if(l>100) l=100;
            return (p,l);
        }
        private static IEnumerable<T> orderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrEmpty(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)? src.OrderByDescending(x=>prop.GetValue(x)):src.OrderBy(x=>prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult getAll([FromQuery] int? page,[FromQuery] int? limit, [FromQuery] string? sort, [FromQuery] string? order)
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Membership> query = memberships;
            query = orderByProp(query,sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new { data, meta = new { page = p, limit = l, total } });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Membership> getById(Guid id)
        {
            var membership = memberships.FirstOrDefault(a=>a.Id==id);
            return membership is null ? NotFound(new { error = "Membership not found", status = 404 }) : membership;

        }

        [HttpPost]
        public ActionResult<Membership> CreateMembership([FromBody]CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.Plan != "basic" && dto.Plan != "pro" && dto.Plan != "premium") return BadRequest();
            if(dto.Status != "active" && dto.Status != "expired" && dto.Plan != "canceled") return BadRequest();

                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    MemberId=Guid.NewGuid(),
                    Plan=dto.Plan.Trim(),
                    Status=dto.Status.Trim()
                };
            memberships.Add(membership);
            return CreatedAtAction(nameof(getById),new { id=membership.Id},membership);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Membership> UpdateMembership(Guid id, [FromBody] UpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            int index = memberships.FindIndex(a => a.Id == id);
            if (dto.Plan != "basic" && dto.Plan != "pro" && dto.Plan != "premium") return BadRequest();
            if (dto.Status != "active" && dto.Status != "expired" && dto.Plan != "canceled") return BadRequest();
            if (index == -1) return NotFound(new { error = "Membership not found", status = 404 });
            var update = new Membership
            {
                Id = id,
                MemberId = dto.MemberId,
                Plan = dto.Plan.Trim(),
                Status = dto.Status.Trim()
            };
            memberships[index] = update;
            return Ok(update);
           
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
