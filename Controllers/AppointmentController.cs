using FirstExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AppointmentController : ControllerBase
    {
        private static readonly List<Appointment> _appointments = new(){
                new Appointment{Id=Guid.NewGuid(),PetId=Guid.NewGuid(),ScheduledAt=DateTime.Now, Reason="vaccinate",Status="scheduled" },
            new Appointment{Id=Guid.NewGuid(),PetId=Guid.NewGuid(),ScheduledAt=DateTime.Now, Reason="vaccinate",Status="scheduled" }
        };

        private static(int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p =1;
            var l = page.GetValueOrDefault(10); if (l < 10) l = 10; if (l > 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) { return src; }
            var prop = typeof(T).GetProperty(sort, BindingFlags.Instance| BindingFlags.Public| BindingFlags.IgnoreCase);
            if(prop==null) { return src; }
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
            query=OrderByProp(query, sort, order);
            var total = query.Count();
            var data = query.Skip((p-1)*l).Take(l).ToList();
            return Ok(new { data, meta = new { page = p, limit = l, total } });

        }

        [HttpGet("{id:Guid}")]
        public ActionResult<Appointment> GetById(Guid id)
        {
            var result=_appointments.FirstOrDefault(x => x.Id == id);
            if (result == null) { return NotFound(new { error = "Appointment Not Found", status = 404 }); }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Appointment> Update([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PetId = Guid.NewGuid(),
                ScheduledAt = DateTime.Now,
                Reason = dto.Reason,
                Status = dto.Status,
                Notes = ""
            };
            if (dto.Notes != null) { appointment.Notes = dto.Notes; }
            _appointments.Add(appointment);
            return CreatedAtAction(nameof(GetOne), new { id = appointment.Id }, appointment};
        }

        [HttpPut("{Id:Guid}")]
        public ActionResult<Appointment> Update(Guid id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
            var index = _appointments.FindIndex(x => x.Id == id);
            if (index == -1) {return NotFound(new { error = "Appointment Not Found", status = 404 }); }
            var update = new Appointment
            {
                Id = id,
                PetId = _appointments[index].PetId,
                Reason = dto.Reason,
                Status = dto.Status
            };
            if (dto.Notes != null) { update.Notes = dto.Notes; }
            _appointments[index] = update;
            return Ok(update);
        }

        [HttpDelete("{Id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var count = _appointments.RemoveAll(x => x.Id == id);
            if (count == 0) { return NotFound(new { error = "Appointment Not Found", status = 404 }); }
            else { return NoContent(); }
        }
    }
}

