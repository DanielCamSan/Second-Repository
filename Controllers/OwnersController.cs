using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
//using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OwnersController:ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner { Id = Guid.NewGuid() , Email = "maria@gmail.com", FullName = "Maria Lozada"},
            new Owner { Id = Guid.NewGuid() , Email = "juan@gmail.com", FullName = "Juan Perez"},
            new Owner { Id = Guid.NewGuid() , Email = "camilo@gmail.com", FullName = "Camilo Rodriguez"},
            new Owner { Id = Guid.NewGuid() , Email = "victor@gmail.com", FullName = "Victor Medrano"}

        };

        private static (int page, int limit) NormalizaPage(int? page, int? limit)
        {

            var p= page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 10; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T> (IEnumerable<T> src, string? sort, string? order)
        {
            if(string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof (T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(prop == null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));

        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page, [FromQuery] int? limit,
            [FromQuery] string? sort, [FromQuery]string? order,
            [FromQuery] string? q, [FromQuery] string? names )

        {
            var (p, l) = NormalizaPage(page, limit);
            IEnumerable<Owner> query = _owners;
            if(!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(o =>
                o.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                o.FullName.Contains(q, StringComparison.OrdinalIgnoreCase);
            }

            if( !string.IsNullOrWhiteSpace(names))
            {
                query = query.Where(o => o.FullName.Equals(names, StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p-1)*l).Take(1).ToList();
            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<Owner> GetOne(Guid id)
        {
            var owner = _owners.FirstOrDefault(o => o.Id == id);
            return owner is null ? NotFound() : Ok(owner);
        }

        [HttpPost]
        public ActionResult<Owner> Create([FromBody] CreateOwnerDto dto)
        {
            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };
            _owners.Add(owner);
            return CreatedAtAction(nameof(GetOne), new { id = owner.Id });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Owner> Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {

            var index = _owners.FindIndex(o => o.Id == id);
            if (index == -1) return NotFound();
            var updated = new Owner
            {
                Id = id,
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };
            _owners[index] = updated;
            return Ok(updated);
        }

    }
}
