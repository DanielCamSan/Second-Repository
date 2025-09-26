using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class OwnersController : Controller
    {
        public static readonly List<Owner> owners = new()
        {
            new Owner { Id = Guid.NewGuid(), Email= "ana@gmail.com",FullName="Ana Pérez" ,Phone="+59177407697", Active= true },
            new Owner { Id = Guid.NewGuid(), Email= "diego@gmail.com",FullName="Diego Castro" ,Phone="+59177777777", Active= true },
            new Owner { Id = Guid.NewGuid(), Email= "juan@gmail.com",FullName="Juan Pérez" ,Phone="+59177777", Active= true },

        };
        private static (int page, int limit) NormalizePage (int? page,int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p<1) p=1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);

        }
        private static IEnumerable<T> OrderbyProp<T>(IEnumerable<T> src,string? sort, string order)
        {
            if (string.IsNullOrEmpty(sort)) return src;
            var prop=typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return src;
            return string.Equals(order,"desc",StringComparison.OrdinalIgnoreCase)?src.OrderByDescending(x=>prop.GetValue(x)):src.OrderBy(x=>prop.GetValue(x));

        }

        [HttpGet]
        public  IActionResult GetAll([FromQuery] int Page, [FromQuery] int limit, [FromQuery] string sort, [FromQuery] string? order, [FromQuery] string? Q )
        {

            var(p,l)= NormalizePage(Page,limit);
            IEnumerable<Owner> query = owners;
            if (!string.IsNullOrEmpty(Q))
            {
                query = query.Where(a=>a.Email.Contains(Q, StringComparison.OrdinalIgnoreCase) || a.FullName.Contains(Q, StringComparison.OrdinalIgnoreCase));
            }
            query=OrderbyProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p-1)-l).Take(l).ToList();
            return Ok(new { data, meta = new { Page = p, limit = l, total } });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owner = owners.FirstOrDefault(a => a.Id == id);
            return owner is null ? NotFound(new { error = "owner not found ", status = 404 }): Ok(owner);  

        }
        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var owner = new Owner()
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Phone = dto.Phone.Trim(),
                Active = dto.Active,
            };

            owners.Add(owner);
            return CreatedAtAction(nameof(GetOne), new {id=owner.Id},owner);
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = owners.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Owner not found", status = 404 });
            var updated = new Owner
            {
                Id = id,
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Phone = dto.Phone.Trim(),
                Active = dto.Active
            };
            owners[index]=updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = owners.RemoveAll(a => a.Id == id);
            return removed == 0 ? NotFound(new { error = "Owner not found", status = 404 }) : NoContent();
        }

    }
}
