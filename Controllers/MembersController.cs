using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MembersController : ControllerBase
    {
        private static List<Member> members = new List<Member>
        {
            new Member { Id = Guid.NewGuid(), Email = "juan@gmail.com", Active = true, FullName= "Juan Perez" },
            new Member { Id = Guid.NewGuid(), Email = "maria@gmail.com", Active = true, FullName= "Maria Lopez" },
            new Member { Id = Guid.NewGuid(), Email = "lucas@gmail.com", Active = true, FullName= "Lucas Soria" },

        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 10; if (l > 100) l = 10;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? q,
            [FromQuery] string? order,
            [FromQuery] string? names
            )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Member> query = members;

            //busqueda libre
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtro específico (Names)
            if (!string.IsNullOrWhiteSpace(names))
            {
                query = query.Where(a => a.FullName.Equals(names, StringComparison.OrdinalIgnoreCase));
            }

            // ↕️ ordenamiento dinámico (safe)
            query = OrderByProp(query, sort, order);

            // 📄 paginación
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });


        }
        [HttpGet("{id:guid}")]
        public ActionResult GetById(Guid id)
        {
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound(new { message = "Member not found", status = 404 });
            }
            return Ok(member);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateMemberDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newMember = new Member
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName,
                Active = true
            };
            members.Add(newMember);
            return CreatedAtAction(nameof(GetById), new { id = newMember.Id }, newMember);
        }

        [HttpPut("{id:guid}")]
        public ActionResult Update(Guid id, [FromBody] UpdateMemberDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound(new { message = "Member not found", status = 404 });
            }
            member.Email = dto.Email;
            member.FullName = dto.FullName;
            member.Active = dto.Active;
            return Ok(member);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound(new { message = "Member not found", status = 404 });
            }
            members.Remove(member);
            return NoContent();
        }


    }
}