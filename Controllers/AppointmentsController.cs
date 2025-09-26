using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> _appointments = new()
        {
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "scheduled", Reason = "control" },
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "completed", Reason = "control" },
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "canceled", Reason = "vacunacion", Notes = "Alergia" }
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = page.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x => prop.GetValue(x))
                : src.OrderBy(x => prop.GetValue(x));

        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int? page,
            [FromQuery] int? limit,
            [FromQuery] string? sort,
            [FromQuery] string? order
            )
        {
            var (p, l) = NormalizePage(page, limit);

            IEnumerable<Appointment> query = _appointments;
            query = OrderByProp(query, sort, order);

            var total = query.Count();
            var data = query.Skip((p - 1) * l).Take(l).ToList();
            return Ok(new
            { data,
                meta = new { page = p, limit = l, total }
            });
        }

        [HttpGet("{id : Guid}")]
        public ActionResult<Appointment> GetOne(Guid id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            return appointment is null 
                ? NotFound(new{ error = "Appointment not found", status = 404})
                : Ok(appointment);
        }

        [HttpPost]
        public ActionResult<Appointment> Create([FromBody] CreateAppointmentDto dto)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PetId = dto.PetId,
                ScheduledAt = DateTime.Now,
                Reason = dto.Reason,
                Status = "scheduled",
                Notes = dto.Notes
            };

            _appointments.Add(appointment);
            return CreatedAtAction(nameof(GetOne), new { id = appointment.Id }, appointment);
        }

    }
}
