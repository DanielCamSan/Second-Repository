using Microsoft.AspNetCore.Mvc;
using FirstExam.Models;
using static FirstExam.Models.Members;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Members> _members = new()
        {
            new Members {Id = Guid.NewGuid(), Email = "fabio@ucb.com", Name = "Fabio Garcia Meza", Active = true },
            new Members {Id = Guid.NewGuid(), Email = "joaco@ucb.com", Name = "Joaquin Elias Soria", Active = true },
            new Members {Id = Guid.NewGuid(), Email = "kat@ucb.com", Name = "Katherine", Active = true }
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
