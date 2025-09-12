using GymApi.Dtos;
using GymApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GymApi.Dtos
{
    public class CreateMemberDto
    {
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool Active { get; set; } = true;
    }
}

namespace GymApi.Dtos
{
    public class UpdateMemberDto
    {
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        public bool Active { get; set; }
    }
}

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembersController : ControllerBase
    {
        // Almacenamiento temporal en memoria (para desarrollo)
        private static readonly List<Member> _members = new();
        private static readonly object _lock = new();

        [HttpGet]
        public ActionResult<object> GetMembers(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? sort = null,
            [FromQuery] string order = "asc")
        {
            // Validar parámetros
            if (page < 1) page = 1;
            if (limit < 1 || limit > 100) limit = 10;
            if (order != "asc" && order != "desc") order = "asc";

            lock (_lock)
            {
                var query = _members.AsQueryable();

                // Aplicar ordenamiento
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort.ToLower())
                    {
                        case "email":
                            query = order == "desc" ? query.OrderByDescending(m => m.Email) : query.OrderBy(m => m.Email);
                            break;
                        case "fullname":
                            query = order == "desc" ? query.OrderByDescending(m => m.FullName) : query.OrderBy(m => m.FullName);
                            break;
                        case "active":
                            query = order == "desc" ? query.OrderByDescending(m => m.Active) : query.OrderBy(m => m.Active);
                            break;
                        default:
                            query = query.OrderBy(m => m.Id);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(m => m.Id);
                }

                var total = query.Count();
                var members = query.Skip((page - 1) * limit).Take(limit).ToList();

                return Ok(new
                {
                    data = members,
                    meta = new { page, limit, total }
                });
            }
        }
    }
}