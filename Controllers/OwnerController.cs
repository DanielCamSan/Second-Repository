using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OwnerController : ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner { Id = Guid.NewGuid(), Email = "JuanPerez@gmail.com", FullName = "Juan Perez", Active = true },
            new Owner { Id = Guid.NewGuid(), Email = "MariaGomez@gmail.com", FullName = "Maria Gomez", Active = true },
            new Owner { Id = Guid.NewGuid(), Email = "CarlosSanchez@gmail.com", FullName = "Carlos Sanchez", Active = false }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ?
                src.OrderByDescending(x => prop.GetValue(x, null)) :
                src.OrderBy(x => prop.GetValue(x, null));
        }


    }
}