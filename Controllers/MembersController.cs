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

    }
}
