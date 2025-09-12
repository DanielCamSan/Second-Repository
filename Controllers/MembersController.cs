using Microsoft.AspNetCore.Mvc;
using FirstExam.Models;
using static FirstExam.Models.Members;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Members> _members = new()
        {
            new Members {Id = Guid.NewGuid(), Email = "fabio@ucb.com", Name = "Fabio Garcia Meza", Active = true },
            new Members {Id = Guid.NewGuid(), Email = "joaco@ucb.com", Name = "Joaquin Elias Soria", Active = true },
            new Members {Id = Guid.NewGuid(), Email = "kat@ucb.com", Name = "Katherine", Active = true }
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
            [FromQuery] string? email
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Members> query = _members;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(m =>
                    m.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    m.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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
        public ActionResult<Members> GetOne(Guid id)
        {
            var member = _members.FirstOrDefault(m => m.Id == id);
            return member is null ? NotFound(new { error = "Member not found", status = 404 }) : Ok(member);
        }
        [HttpPost]
        public ActionResult<Members> Create([FromBody] CreateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var member = new Members
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                Name = dto.Name.Trim(),
                Active = dto.IsActive
            };
            _members.Add(member);
            return CreatedAtAction(nameof(GetOne), new { id = member.Id }, member);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Members> Update(Guid id, [FromBody] UpdateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _members.FindIndex(m => m.Id == id);
            if (index == -1) return NotFound(new { error = "Member not found", status = 404 });

            var updated = new Members
            {
                Id = id,
                Email = dto.Email.Trim(),
                Name = dto.Name.Trim(),
                Active = dto.IsActive
            };

            _members[index] = updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _members.RemoveAll(m => m.Id == id);
            return removed == 0 ? NotFound(new { error = "Member not found", status = 404 }) : NoContent();
        }
    }
}
