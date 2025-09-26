using Microsoft.AspNetCore.Mvc;
using FirstExam.Models;
using FirstExam.Models.DTOs;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OwnersController : ControllerBase
    {
        private static readonly List<Owner> _owners = new();
        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? sort = null, [FromQuery] string? order = "asc")
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;
            if (limit > 100) limit = 100;
            if (order != "asc" && order != "desc") order = "asc";
            IEnumerable<Owner> query = _owners;
            query = sort switch
            {
                "fullName" => order == "asc" ? query.OrderBy(o => o.FullName) : query.OrderByDescending(o => o.FullName),
                "email" => order == "asc" ? query.OrderBy(o => o.Email) : query.OrderByDescending(o => o.Email),
                _ => query.OrderBy(o => o.Id)
            };
            var total = query.Count();
            var items = query.Skip((page - 1) * limit).Take(limit).ToList();
            return Ok(new
            {
                data = items,
                meta = new { page, limit, total }
            });
        }
        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var owner = _owners.FirstOrDefault(o => o.Id == id);
            return owner == null
                ? NotFound(new { error = "Owner not found", status = 404 })
                : Ok(owner);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOwnerDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var owner = new Owner
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                Active = dto.Active
            };

            _owners.Add(owner);

            return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
        }
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateOwnerDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existing = _owners.FirstOrDefault(o => o.Id == id);
            if (existing == null)
                return NotFound(new { error = "Owner not found", status = 404 });

            existing.Email = dto.Email.Trim();
            existing.FullName = dto.FullName.Trim();
            existing.Active = dto.Active;

            return Ok(existing);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var existing = _owners.FirstOrDefault(o => o.Id == id);
            if (existing == null)
                return NotFound(new { error = "Owner not found", status = 404 });

            _owners.Remove(existing);
            return NoContent();
        }
    }
}
