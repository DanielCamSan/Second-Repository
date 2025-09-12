using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "j.deer@gmail.com" , FullName = "John Deer", Active = true},
            new Member { Id = Guid.NewGuid(), Email = "jane.doe@gmail.com" , FullName = "Jane Doe", Active = true},
            new Member { Id = Guid.NewGuid(), Email = "andy.andrade@gmail.com" , FullName = "Andres Andrade", Active = true},
            new Member { Id = Guid.NewGuid(), Email = "dahbner@gmail.com" , FullName = "Dabner Orozco", Active = false}
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
            [FromQuery] string? order
            )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Member> query = _members;
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
            var member = _members.FirstOrDefault(x => x.Id == id);
            return member is null 
                ? NotFound(new { error = "Member not found", status = 404})
                : Ok(member);
        }


        [HttpPost]
        public ActionResult<Member> Create([FromBody] CreateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var member = new Member
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = true
            };

            _members.Add(member);
            return CreatedAtAction(nameof(GetOne), new { id = member.Id }, member);
        }


        [HttpPut]
        public ActionResult<Member> Update(Guid id, [FromBody] UpdateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _members.FindIndex(x => x.Id == id);
            if (index == -1)
                return NotFound(new { error = "Member not found", status = 404 });

            var updated = new Member
            {
                Id = id,
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = true
            };

            _members[index] = updated;
            return Ok(updated);
        }


        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _members.RemoveAll(x => x.Id == id);
            return removed == 0
                ? NotFound(new { error = "Member not found", status = 404 })
                : NoContent();
        }
    }
}
