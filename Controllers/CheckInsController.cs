using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private static readonly List<CheckIn> _checkIns = new()
        {
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.UtcNow },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-54321", Timestamp = DateTime.UtcNow.AddMinutes(-30) },
            new CheckIn { Id = Guid.NewGuid(), BadgeCode = "GYM-99999", Timestamp = DateTime.UtcNow.AddHours(-1) }
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