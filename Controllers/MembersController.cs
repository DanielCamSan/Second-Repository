using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class membersController : ControllerBase
    {
        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "lucas.alcoba@example.com", FullName = "Lucas Alcoba", Active = true },
            new Member { Id = Guid.NewGuid(), Email = "ana.torres@example.com", FullName = "Ana Torres", Active = true },
            new Member { Id = Guid.NewGuid(), Email = "carlos.mendoza@example.com", FullName = "Carlos Mendoza", Active = false },
            new Member { Id = Guid.NewGuid(), Email = "maria.lopez@example.com", FullName = "María López", Active = true },
            new Member { Id = Guid.NewGuid(), Email = "andres.perez@example.com", FullName = "Andrés Pérez", Active = false }

        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src; // no-op
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; // campo inválido => no ordenar

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,     // ejemplo: name | species | age
            [FromQuery] string? order,    // asc | desc
            [FromQuery] string? q,        // búsqueda en Name/Species (contains)
            [FromQuery] string? species   // filtro exacto por especie (Dog/Cat/...)
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Member> query = _members;

            // 🔎 búsqueda libre (Name/Species)
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // 🧭 filtro específico (Species)
            if (!string.IsNullOrWhiteSpace(species))
            {
                query = query.Where(a => a.Email.Equals(species, StringComparison.OrdinalIgnoreCase));
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

        // READ: GET api/animals/{id}

        [HttpGet("{id:guid}")]
        public ActionResult<Member> GetOne(Guid id)
        {
            var user = _members.FirstOrDefault(a => a.Id == id);
            return user is null
                ? NotFound(new { error = "User not found", status = 404 })
                : Ok(user);
        }
        // CREATE: POST api/animals
        [HttpPost]
        public ActionResult<Member> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new Member
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };

            _members.Add(user);
            return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
        }

        // UPDATE: PUT api/animals/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Member> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _members.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "User not found", status = 404 });

            var updated = new Member
            {
                Id = id,
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active

            };

            _members[index] = updated;
            return Ok(updated);
        }

        // DELETE: DELETE api/animals/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _members.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "User not found", status = 404 })
                : NoContent();
        }
    }

    
}



