using Microsoft.AspNetCore.Mvc;
using System.Reflection;
namespace Appointments.Controller

{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController:ControllerBase
    {
        public static readonly List<Appointment> _appointments = new()

        {
            ///vacunación, control, cirugía, etc. 
            new Appointment { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = System.DateTime.Now, Reason = "Vacunacion", Status = "Scheduled", Notes = "N/A" },
            new Appointment { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = System.DateTime.Now, Reason = "control", Status = "Scheduled", Notes = "N/A" },
            new Appointment { Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = System.DateTime.Now, Reason = "cirugia", Status = "Scheduled", Notes = "N/A" },

        }; 
        private static(int page, int limit) NormalizePage(int? page , int? limit)
        {
            var p = page.GetValueOrDefault(1); if(p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if(l < 1) l = 10; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop is null) return src; 
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase) ?
                src.OrderByDescending(x => prop.GetValue(x)) :
                src.OrderBy(x => prop.GetValue(x));
        }

        [HttpGet]
        public IActionResult getAll(
            [FromQuery] int? page, 
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order,
            [FromQuery] string? q
            )
        {
            var (p, l) = NormalizePage(page, limit);
            IEnumerable<Appointment> query = _appointments;
            if(!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => x.Reason.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                         x.Status.Contains(q, StringComparison.OrdinalIgnoreCase)
                                         );
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
        [HttpGet("{id:guid}")]
        public ActionResult<Appointment>getById(Guid id)
        {
            var appointment = _appointments.FirstOrDefault(x => x.Id == id); 
            return appointment is null? NotFound(new {error = "Appointmen not found", status = 404 }) : Ok(appointment);
        }

        [HttpPost]
        public ActionResult<Appointment> create([FromBody] CreateAppointmentDto dto)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PetId = Guid.NewGuid(),
                ScheduledAt = DateTime.Now, 
                Reason = dto.Reason,
            };
            _appointments.Add(appointment);
            return CreatedAtAction(nameof(getById), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Appointment> update( Guid id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var index = _appointments.FindIndex(x => x.Id == id);
            if (index == -1) return NotFound(new { error = "Appointment not found", status = 404 });
            var updated = new Appointment
            {
                Id = id,
                PetId = _appointments[index].PetId,
                ScheduledAt = dto.ScheduledAt,
                Status = dto.Status,
                Reason = dto.Reason,
                Notes = dto.Notes,
            };
            _appointments[index] = updated;
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult delete(Guid id )
        {
            var delete = _appointments.RemoveAll(x => x.Id == id);
            return delete == 0 ? NotFound(new { error = "Appointment not found", status = 404 }) : NoContent();
        }
    }

}
