using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Controllers{

    [ApiController]
    [Route ("api/[controller]")]
    public class MembershipsController:ControllerBase{
        private static readonly List<Membership> _memberships = new() {
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "basic", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(2), Status = "active" },
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "pro", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6), Status = "expired" },
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "premium", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4), Status = "canceled" },
        };

        private static(int page, int limit)NormalizePage(int? page, int? limit)
        {
            var p=page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar
            return string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderBy(x => prop.GetValue(x))
                : src.OrderByDescending(x => prop.GetValue(x));
        }

        // GET api/memberships  (con paginación + ordenamiento)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     
            [FromQuery] string? order
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Membership> query = _memberships;

            // ordenamiento
            query = OrderByProp(query, sort, order);

            // paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // READ: GET api/memberships/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = _memberships.FirstOrDefault(a => a.Id == id);
            return membership is null
                ? NotFound(new { error = "Membership not found", status = 404 })
                : Ok(membership);
        }

        // CREATE: POST api/memberships
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

            _memberships.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }

        // UPDATE: PUT api/memberships/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _memberships.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Membership not found", status = 404 });

            var updated = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = Guid.NewGuid(),
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
            };

            _memberships[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/memberships/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _memberships.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Membership not found", status = 404 })
                : NoContent();
        }
    };
}