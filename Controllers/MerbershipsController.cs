using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private static readonly List<Membership> _memberships = new()
        {
            new Membership{ Id = Guid.NewGuid(), MemberId = Guid.Parse("00000000-0000-0000-0000-000000000001"), Plan = "pro",
                StartDate = new DateTime(2025, 9, 10), EndDate = new DateTime(2026, 9, 10), Status = "active"},

            new Membership
            {
                Id = Guid.NewGuid(), MemberId = Guid.Parse("00000000-0000-0000-0000-000000000002"), Plan = "basic",
                StartDate = new DateTime(2024, 5, 1), EndDate = new DateTime(2025, 5, 1), Status = "expired" },

            new Membership
            {
                Id = Guid.NewGuid(), MemberId = Guid.Parse("00000000-0000-0000-0000-000000000003"), Plan = "premium",
                StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2026, 1, 15), Status = "active" }
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
        [FromQuery] string? sort,       // example: plan | status | startDate
        [FromQuery] string? order     // asc | desc
        )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Membership> query = _memberships;


            // dynamic sorting
            query = OrderByProp(query, sort, order);

            // pagination
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }

        // READ: GET api/memberships/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Membership> GetOne(Guid id)
        {
            var membership = _memberships.FirstOrDefault(a => a.Id == id);
            return membership is null
                ? NotFound(new { error = "Membership not found", status = 404 })
                : Ok(membership);
        }

        // CREATE: POST api/memberships
        [HttpPost]
        public ActionResult<Membership> Create([FromBody] CreateMembershipDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                MemberId = dto.MemberId,
                Plan = dto.Plan,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };

            _memberships.Add(membership);
            return CreatedAtAction(nameof(GetOne), new { id = membership.Id }, membership);
        }


    }
};