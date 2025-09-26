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
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owner = _owners.FirstOrDefault(a => a.Id == id);
            return owner is null
                ? NotFound(new { error = "Owner not found ", status = 404 })
                : Ok(owner);
        }


        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Active = true
            };
            _owners.Add(owner);
            return CreatedAtAction(nameof(GetOne), new {id=owner.Id},owner);

        }
        [HttpPut("{id: Guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _owners.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound(new { error = "Owner not found", status = 404 });

            var updated = new Owner
            {
                Id = id,
                Email = dto.Email
            };

            _owners[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id : Guid}")]
        public ActionResult Delete(Guid id)
        {
            var removed = _owners.RemoveAll(a => a.Id == id);
            return removed == 0
                ? NotFound(new { error = "Owner not found", status = 404 })
                : NoContent();
        }
    }
    


}
