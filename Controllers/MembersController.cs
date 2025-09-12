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



    }
}
