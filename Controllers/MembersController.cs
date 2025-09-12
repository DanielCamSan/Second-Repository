using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Globalization;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route ("api/[Controller]")]
    public class MembersController
    {
        /*

GET /api/v1/members/{id}

POST /api/v1/members

PUT /api/v1/members/{id}

DELETE /api/v1/members/{id}
         */

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

    }
}
