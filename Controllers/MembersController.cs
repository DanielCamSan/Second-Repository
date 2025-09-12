using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route ("api/v1/[Controller]")]
    public class MembersController:ControllerBase
    {
        

        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "carlal@gmail.com",FullName="carla lopez",active=true},
            new Member { Id = Guid.NewGuid(), Email = "mat@gamil.com",FullName="matias carballo",active=false},
            new Member { Id = Guid.NewGuid(), Email = "rodrigo@gmail.com",FullName="rodrigo soria",active=true},
            new Member { Id = Guid.NewGuid(), Email = "rosa@gamil.com",FullName="rosaura martinez",active=true}
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        //ordenamiento dinamico
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }
        // GET /api/v1/members (lista con paginación, orden)
        [HttpGet]
        public IActionResult GetAll(
           [FromQuery] int? page,
           [FromQuery] int? limit,
           [FromQuery] string? sort,     // ejemplo: name | species | age
           [FromQuery] string? order,    // asc | desc
           [FromQuery] string? q       // búsqueda por nombre o email
       )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Member> query = _members;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase));
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
        //GET /api/v1/members/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Member> GetOne(Guid id)
        {
            var member = _members.FirstOrDefault(a => a.Id == id);
            return member is null
                ? NotFound(new { error = "Member not found", status = 404 })
                : Ok(member);
        }
        //POST /api/v1/members
        [HttpPost]
        public ActionResult<Member> Create([FromBody] CreateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var member = new Member
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName.Trim(),
                active = dto.active
            };

            _members.Add(member);
            return CreatedAtAction(nameof(GetOne), new { id = member.Id }, member);
        }
        //PUT /api/v1/members/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Member> Update(Guid id, [FromBody] UpdateMemberDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _members.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Member not found", status = 404 });

            var updated = new Member
            {
                Id=id,
                Email = dto.Email,
                FullName = dto.FullName.Trim(),
                active = dto.active
            };

            _members[index] = updated;
            return Ok(updated);
        }
        //DELETE /api/v1/members/{id}
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
