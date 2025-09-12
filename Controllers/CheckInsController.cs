using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;


namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckInsController: ControllerBase
    {
        private static readonly List<CheckIns> CheckIns = new()
        {
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "ABC123", Timestamp = DateTime.Now },
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "DEF456", Timestamp = DateTime.Now },
            new CheckIns { Id = Guid.NewGuid(), BadgeCode = "GHI789", Timestamp = DateTime.Now }
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
            if(prop == null) return src;
            if (string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));


        }



    }
}