using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Member> _members = new()
        {
            new Member {Id=Guid.NewGuid(), Email="ana@example.com", FullName="Ana Pérez", Active=true },
            new Member {Id=Guid.NewGuid(), Email="diego@example.com", FullName="Diego Pérez", Active=false },

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
            var prop = typeof(T).GetProperty(sort, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
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
            [FromQuery] string? Email,
            [FromQuery] string? FullName,
            [FromQuery] string? Active
        )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Member> query = _members;
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a => a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Active.ToString().Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(FullName))
            {
                query = query.Where(a => a.FullName.Equals(FullName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                query = query.Where(a => a.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(Active))
            {
                query = query.Where(a => a.Active.ToString().Equals(Active, StringComparison.OrdinalIgnoreCase));
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
        public ActionResult<Member> GetOne(Guid id)
        {
            var member = _members.FirstOrDefault(a => a.Id == id);
            return member is null
                ? NotFound(new { error = "Member not found", status = 404 })
                : Ok(member);

        }
        [HttpPost]
        public ActionResult<Member> Create([FromBody] CreateMemberDto dto){
            if(!ModelState.IsValid){
                return ValidationProblem(ModelState);
            }
            var member = new Member
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName.Trim(),
                Email = dto.Email,
                Active=dto.Active
            };
            _members.Add(member);
            return CreatedAtAction(nameof(GetOne), new { id = member.Id }, member);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Member> Update(Guid id, [FromBody] UpdateMemberDto dto){
            if(!ModelState.IsValid){
                return ValidationProblem(ModelState);
            }
            var index = _members.FindIndex(a => a.Id == id);
            if (index==-1){
                return NotFound(new { error = "Member not found", status = 404 });
            }
            var updated = new Member
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName.Trim(),
                Email = dto.Email,
                Active = dto.Active
            };
            _members[index] = updated;
            return Ok(updated);

        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _members.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Member not found", status = 404 })
                : NoContent();
        }




    }
}