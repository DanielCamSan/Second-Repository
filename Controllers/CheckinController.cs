using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckinController : ControllerBase
    {
        private static readonly List<Checkin> _checkins = new()
        {
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.Now() },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-45788", Timestamp = DateTime.UtcNow() },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-67811", Timestamp = DateTime.UtcNow() }
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
/*
 * CreateCheckInDto
UpdateCheckInDto

GET /api/v1/checkins (lista con paginación, orden)

GET /api/v1/checkins/{id}

POST /api/v1/checkins

DELETE /api/v1/checkins/{id}

 */
