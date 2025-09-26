using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;



namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        public static readonly List<Appointments> _appointments = new()
        {
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="control",Status="completed"},
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="vaccination",Status="scheduled", Notes="ok"},
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="cirugise",Status="canceled", Notes="hard"}
        };
         
        private static (int page, int limit) NormalizePagination(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort,BindingFlags.IgnoreCase|BindingFlags.Public|BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x=>prop.GetValue(x))
                : src.OrderBy(x=>prop.GetValue(x));
        }

    }
}
