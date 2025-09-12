using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        // Almacenamiento en memoria (para el challenge)
        private static readonly List<Membership> _memberships = new()
        {
            new Membership {
                MemberId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Plan = "pro",
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow.AddMonths(11),
                Status = "active"
            },
            new Membership {
                MemberId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Plan = "basic",
                StartDate = DateTime.UtcNow.AddMonths(-6),
                EndDate = DateTime.UtcNow.AddMonths(-1),
                Status = "expired"
            }
        };

        private static readonly HashSet<string> AllowedSort = new(
            new[] { "plan", "status", "startDate", "endDate", "memberId" },
            StringComparer.OrdinalIgnoreCase
        );

        // GET /api/v1/memberships?sort=&order=&page=&limit=
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? sort = "startDate",
            [FromQuery] string? order = "asc")
        {
            // Normalización
            page = page < 1 ? 1 : page;
            limit = limit < 1 ? 10 : (limit > 100 ? 100 : limit);
            order = string.IsNullOrWhiteSpace(order) ? "asc" : order.ToLower();

            sort = string.IsNullOrWhiteSpace(sort) ? "startDate" : sort;
            if (!AllowedSort.Contains(sort)) sort = "startDate";

            // Orden
            IQueryable<Membership> q = _memberships.AsQueryable();
            q = sort.ToLower() switch
            {
                "plan" => order == "desc" ? q.OrderByDescending(x => x.Plan) : q.OrderBy(x => x.Plan),
                "status" => order == "desc" ? q.OrderByDescending(x => x.Status) : q.OrderBy(x => x.Status),
                "enddate" => order == "desc" ? q.OrderByDescending(x => x.EndDate) : q.OrderBy(x => x.EndDate),
                "memberid" => order == "desc" ? q.OrderByDescending(x => x.MemberId) : q.OrderBy(x => x.MemberId),
                _ => order == "desc" ? q.OrderByDescending(x => x.StartDate) : q.OrderBy(x => x.StartDate),
            };

            var total = q.Count();
            var items = q.Skip((page - 1) * limit).Take(limit).ToList();

            return Ok(new
            {
                data = items,
                meta = new { page, limit, total }
            });
        }

        // GET /api/v1/memberships/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var m = _memberships.FirstOrDefault(x => x.Id == id);
            return m is null
                ? NotFound(new { error = "Membership not found", status = 404 })
                : Ok(m);
        }

        // POST /api/v1/memberships
        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.EndDate <= dto.StartDate)
                return BadRequest(new { error = "EndDate must be after StartDate", status = 400 });

            var entity = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "active" : dto.Status
            };

            _memberships.Add(entity);
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity); // 201
        }

        // PUT /api/v1/memberships/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.EndDate <= dto.StartDate)
                return BadRequest(new { error = "EndDate must be after StartDate", status = 400 });

            var idx = _memberships.FindIndex(x => x.Id == id);
            if (idx == -1)
                return NotFound(new { error = "Membership not found", status = 404 });

            var updated = new Membership
            {
                Id = id,
                MemberId = _memberships[idx].MemberId, // no se edita MemberId en Update
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };

            _memberships[idx] = updated;
            return Ok(updated); // 200
        }

        // DELETE /api/v1/memberships/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed = _memberships.RemoveAll(x => x.Id == id);
            return removed == 0
                ? NotFound(new { error = "Membership not found", status = 404 })
                : NoContent(); // 204
        }
    }
}
