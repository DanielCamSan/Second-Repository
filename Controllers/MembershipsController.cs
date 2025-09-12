using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FirstExam.Models;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private static readonly List<Membership> _memberships = new()
        {
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "pro", StartDate = DateTime.UtcNow.AddMonths(-6), EndDate = DateTime.UtcNow.AddMonths(6), Status = "active" },
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "basic", StartDate = DateTime.UtcNow.AddMonths(-1), EndDate = DateTime.UtcNow.AddMonths(11), Status = "active" },
            new Membership { Id = Guid.NewGuid(), MemberId = Guid.NewGuid(), Plan = "premium", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddDays(-1), Status = "expired" }
        };

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? sort = "startDate",
            [FromQuery] string? order = "asc")
        {
            IEnumerable<Membership> query = _memberships;

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var prop = typeof(Membership).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    query = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(x => prop.GetValue(x))
                        : query.OrderBy(x => prop.GetValue(x));
                }
            }

            if (page < 1) page = 1;
            if (limit < 1) limit = 1;
            if (limit > 100) limit = 100;

            var total = query.Count();
            var data = query.Skip((page - 1) * limit).Take(limit).ToList();

            return Ok(new { data, meta = new { page, limit, total } });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = _memberships.FirstOrDefault(m => m.Id == id);
            return membership is null
                ? NotFound(new { error = "Membership not found", status = 404 })
                : Ok(membership);
        }

        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate.ToUniversalTime(),
                EndDate = dto.EndDate.ToUniversalTime(),
                Status = dto.Status
            };

            _memberships.Add(membership);

            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Membership> Update(Guid id, [FromBody] UpdateMembershipDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var index = _memberships.FindIndex(m => m.Id == id);
            if (index == -1)
            {
                return NotFound(new { error = "Membership not found", status = 404 });
            }

            var existingMembership = _memberships[index];
            var updated = new Membership
            {
                Id = id,
                MemberId = existingMembership.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate.ToUniversalTime(),
                EndDate = dto.EndDate.ToUniversalTime(),
                Status = dto.Status
            };

            _memberships[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removedCount = _memberships.RemoveAll(m => m.Id == id);
            return removedCount == 0
                ? NotFound(new { error = "Membership not found", status = 404 })
                : NoContent();
        }
    }
}