using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Appointment;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        public static readonly List<Appointment> appointments = new()
        {
            new Appointment() { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now , Reason = "a", Notes = "a"},
            new Appointment() { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now , Reason = "b", Notes = "b"}
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrEmpty(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return src;

            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ? src.OrderByDescending(x => prop.GetValue(x)) : src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? Page, [FromQuery] int? Limit, [FromQuery] string? Sort, [FromQuery] string? Order, [FromQuery] string? q)
        {
            var (p, l) = NormalizePage(Page, Limit);
            IEnumerable<Appointment> query = appointments;
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(a => a.Reason.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
            query= OrderByProp (query, Sort, Order);
            var total = query.Count();
            var data = query.Skip((p-1)*l).Take(l).ToList();
            return Ok(new { data, meta = new { Page = p, Limit = l, total } });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Appointment> GetOne(Guid id)
        {
            var appointment = appointments.FirstOrDefault(a => a.Id == id);
            return appointment is null? NotFound(new { error = "Appointment not found", status = 404}): Ok(appointment);
        }

        [HttpPost]
        public ActionResult<Appointment> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var appointment = new Appointment { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Reason = dto.Reason.Trim(), Notes = dto.Notes.Trim(), Status = dto.Status.Trim() };
            appointments.Add(appointment);
            return CreatedAtAction(nameof(GetOne), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Appointment> Update(Guid id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = appointments.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound(new { error = "Appointment not found", status = 404 });
            var updated = new Appointment
            {
                Id = id,
                PetId = dto.PetId,
                ScheduledAt = DateTime.Now,
                Reason = dto.Reason.Trim(),
                Status = dto.Status.Trim(),
                Notes = dto.Notes.Trim()
            };

            appointments[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult<Appointment> Delete(Guid id)
        {
            var removed = appointments.RemoveAll(a => a.Id == id);
            return removed == 0? NotFound(new { error = "Appointment not found", status =404 }): NoContent();
        }
    }   
}
  