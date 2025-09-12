using System.Net.Security;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Members.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Member> members = new()
        {
            new Member {Id = Guid.NewGuid(), FullName = "Ramiro", Email= "Ramiro@gmail.com" },
            new Member {Id = Guid.NewGuid(), FullName = "Daniel", Email= "Daniel@gmail.com"},
            new Member {Id = Guid.NewGuid(), FullName = "Sebastian", Email= "Sebastian@gmail.com"},
            new Member {Id = Guid.NewGuid(), FullName = "Luis", Email= "Luis@gmail.com"},
            new Member {Id = Guid.NewGuid(), FullName = "Alejandro", Email= "Alejandro@gmail.com"}
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
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ? src.OrderByDescending(x => prop.GetValue(x)) :
            src.OrderBy(x => prop.GetValue(x));

        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Member> query = members;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a =>
                    a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(
                new
                {
                    data,
                    meta = new { page = p, limit = l, total }
                });
        }

        ///GET /api/v1/memberships/{id}

        [HttpGet("{id:guid}")]
        public ActionResult<Member> GetOne(Guid id)
        {
            var member = members.FirstOrDefault(a => a.Id == id); 
            return member is null?  NotFound(new { error = "Member not found", status = 404 }): Ok(member); 
        }


    }
}