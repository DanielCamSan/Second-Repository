using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[Controlller]")]
    public class OwnersController : ControllerBase 
    {
        private static readonly List<Owner> _owners = new()
        {
        new Owner {Id=Guid.NewGuid(),Email="flora@gmail.com",FullName="Flora Ortiz",Active=true},
        new Owner {Id=Guid.NewGuid(),Email="Dabner@gmail.com",FullName="Dabner Orozco",Active=true},
        new Owner {Id=Guid.NewGuid(),Email="Andy@gmail.com",FullName="Ricard Andrade",Active=true},

        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = page.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));

        }
        //Get
        [HttpGet]
        public ActionResult Getall(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order
            )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Owner> query = _owners;
            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new
            {
                data,
                meta = new
                {
                    page = p,
                    limit = l,
                    total
                }
            });
        }
        [HttpGet("{id:guid}")]

        public ActionResult<Owner>
    }
    


}
