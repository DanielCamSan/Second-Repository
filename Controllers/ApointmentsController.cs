using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Reflection;


namespace FirstExam.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class ApointmentsController: ControllerBase
    {
        private static readonly List<Apointment> _apointments = new()
        {
            new Apointment{Id=Guid.NewGuid(), ScheduledAt=DateTime.Now, Reason="No one", Status="Completed", Notes="The pacient is not sick" },
            new Apointment{Id=Guid.NewGuid(), ScheduledAt=DateTime.Now, Reason="No one", Status="Completed", Notes="The pacient is not sick" },
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if(p<1) p = 1;
            var l = limit.GetValueOrDefault(10); if(l<1) l = 1; if(l<100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (!string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
            ? src.OrderByDescending(x=> prop.GetValue(x))
            : src.OrderBy(x=> prop.GetValue(x));
        }
        /*
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page, [FromQuery] int? limit,
            [FromQuery] string? sort, [FromQuery] string? order,
            [FromQuery] string? q, [FromQuery] DateTime scheduledAt)
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Apointment> query = _apointments;
            scheduledAt.ToString();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a => a.Reason.Contains(q, StringComparison.OrdinalIgnoreCase) || a.scheduledAt.ToString().Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            if (!StringComparison(scheduledAt.ToString()))
            {
                query = query.Where(a => a.Equals(scheduledAt.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            query = OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();

            return Ok(new
            {
                data,
                meta = new { page = p, limit = l, total }
            });
        }
        */
            [HttpGet("{id:guid}")]
            public ActionResult<Apointment>GetOne(Guid id) 
            {
                    var apointment=_apointments.FirstOrDefault(a => a.Id == id);
                    return apointment is null ? NotFound() : Ok(apointment);
            }
        [HttpPost]
        public ActionResult<Apointment> Create([FromBody] CreateApointmentDto dto) 
        {
            var apointment = new Apointment
            {
                Id = Guid.NewGuid(),
                ScheduledAt = dto.ScheduledAt,
                Reason = dto.Reason,
                Status = dto.Status,
                Notes = dto.Notes,
            };
            _apointments.Add(apointment);
            return CreatedAtAction(nameof(GetOne), new {id=apointment.Id});
        }
        [HttpPut("{id:guid}")]
        public ActionResult<Apointment> Update(Guid id, [FromBody] UpdateApointmentDto dto)
        {
            var index = _apointments.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();
            var updated = new Apointment
            {
                Id = Guid.NewGuid(),
                ScheduledAt = dto.ScheduledAt,
                Reason = dto.Reason,
                Status = dto.Status,
                Notes = dto.Notes,
            };
            _apointments[index] = updated;
            return Ok(updated);
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var removed =_apointments.RemoveAll(a => a.Id == id);
            return  removed == 0 ? NotFound() : NoContent();
        }
    }
}
