using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
        {
            new Pet{Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(),Name="a",Species="dog",Breed="a",Birthday=DateTime.Parse("12-05-2010"),sex="f",WeightKg=7},
            new Pet{Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(),Name="b",Species="cat",Breed="b",Birthday=DateTime.Parse("12-05-2011"),sex="m",WeightKg=8},
            new Pet{Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(),Name="c",Species="bird",Breed="c",Birthday=DateTime.Parse("12-05-2012"),sex="f",WeightKg=9},
            new Pet{Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(),Name="d",Species="reptile",Breed="d",Birthday=DateTime.Parse("12-05-2013"),sex="m",WeightKg=10},
            new Pet{Id=Guid.NewGuid(),OwnerId=Guid.NewGuid(),Name="e",Species="other",Breed="e",Birthday=DateTime.Parse("12-05-2014"),sex="f",WeightKg=1}
        };
        private static (int page ,int limit) NormalizePage(int? page,int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1;if (l > 100) l = 100;
            return (p,l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src,string? sort,string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x=>prop.GetValue(x));
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] int? page,
                                    [FromQuery] int? limit,
                                    [FromQuery] string? sort,
                                    [FromQuery] string? order,
                                    [FromQuery] string? q
            )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Pet> query = _pets;
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p =>
                    p.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    p.Species.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new
            { data,
              meta = new {page=p, limit=l, total} 
            });
        }
                                
    }

}
