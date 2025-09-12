using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        // Datos en memoria (demo)
        private static readonly List<Membership> _memberships = new()
        {
            new Membership { Id = 1, MemberId = 100, Plan = "pro",     StartDate = DateTime.UtcNow.AddMonths(-2),  EndDate = DateTime.UtcNow.AddMonths(4),  Status = "active"  },
            new Membership { Id = 2, MemberId = 101, Plan = "premium", StartDate = DateTime.UtcNow.AddMonths(-12), EndDate = DateTime.UtcNow.AddMonths(-1), Status = "expired" },
            new Membership { Id = 3, MemberId = 102, Plan = "basic",   StartDate = DateTime.UtcNow,               EndDate = DateTime.UtcNow.AddMonths(1),  Status = "active"  }
        };

        // GET https://localhost:7047/api/memberships
        [HttpGet]
        public ActionResult<IEnumerable<Membership>> GetAll()
        {
            return Ok(_memberships);
        }

        // GET https://localhost:7047/api/memberships/1
        [HttpGet("{id:int}")]
        public ActionResult<Membership> GetOne(int id)
        {
            var ms = _memberships.FirstOrDefault(m => m.Id == id);
            return ms is null ? NotFound() : Ok(ms);
        }

        // POST https://localhost:7047/api/memberships
        [HttpPost]
        public ActionResult<Membership> Create([FromBody] Membership membership)
        {
            if (membership is null) return BadRequest();

            // Asigna Id incremental
            membership.Id = _memberships.Count == 0 ? 1 : _memberships.Max(m => m.Id) + 1;

            // Defaults
            if (string.IsNullOrWhiteSpace(membership.Plan)) membership.Plan = "basic";
            if (string.IsNullOrWhiteSpace(membership.Status)) membership.Status = "active";

            _memberships.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }

        // PUT https://localhost:7047/api/memberships/2
        [HttpPut("{id:int}")]
        public ActionResult<Membership> Update(int id, [FromBody] Membership membership)
        {
            if (membership is null) return BadRequest();

            var index = _memberships.FindIndex(m => m.Id == id);
            if (index == -1) return NotFound();

            // Conserva el Id del recurso
            membership.Id = id;
            _memberships[index] = membership;

            return Ok(membership);
        }

        // PATCH https://localhost:7047/api/memberships/3
        [HttpPatch("{id:int}")]
        public ActionResult<Membership> Patch(int id, [FromBody] Membership partial)
        {
            if (partial is null) return BadRequest();

            var membership = _memberships.FirstOrDefault(m => m.Id == id);
            if (membership is null) return NotFound();

            // Actualiza solo si viene valor
            if (!string.IsNullOrWhiteSpace(partial.Plan)) membership.Plan = partial.Plan;
            if (!string.IsNullOrWhiteSpace(partial.Status)) membership.Status = partial.Status;
            if (partial.MemberId != 0) membership.MemberId = partial.MemberId;
            if (partial.StartDate != default) membership.StartDate = partial.StartDate;
            if (partial.EndDate != default) membership.EndDate = partial.EndDate;

            return Ok(membership);
        }

        // DELETE https://localhost:7047/api/memberships/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var removed = _memberships.RemoveAll(m => m.Id == id);
            return removed == 0 ? NotFound() : NoContent();
        }
    }
}
