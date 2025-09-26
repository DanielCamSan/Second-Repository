using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pets> _pets = new() {
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Rodrigo",Species="dog",Breed="german", BirthData = DateTime.Now,Sex= "female",WeightKg=55},
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Adrian",Species="dog",Breed="bulldog", BirthData = DateTime.Now,Sex= "male",WeightKg=80},
           new Pets {Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(), Name = "Randall",Species="dog",Breed="pudle", BirthData = DateTime.Now,Sex= "female",WeightKg=65},
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

        
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q,
            [FromQuery] string? name
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Pets> query = _pets;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();

                var esNumero = int.TryParse(term, out var dur);
                var esFecha = DateTime.TryParse(term, out var dt);

                query = query.Where(a =>
                    a.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (esNumero && a.WeightKg == dur) ||
                    (esFecha && a.BirthData.Date == dt.Date)
                );
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }
        








    };


   
    





}