using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> _appointments = new();

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound(new { error = "Appointment not found", status = 404 });

            return Ok(appointment);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Appointment.CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PetId = dto.PetId,
                ScheduledAt = dto.ScheduledAt,
                Reason = dto.Reason,
                Status = dto.Status ?? "scheduled",
                Notes = dto.Notes
            };

            _appointments.Add(appointment);

            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Appointment.UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound(new { error = "Appointment not found", status = 404 });

            appointment.ScheduledAt = dto.ScheduledAt;
            appointment.Reason = dto.Reason;
            appointment.Status = dto.Status;
            appointment.Notes = dto.Notes;

            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound(new { error = "Appointment not found", status = 404 });

            _appointments.Remove(appointment);
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string sort = "scheduledAt",
        [FromQuery] string order = "asc")
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;
            if (limit > 100) limit = 100;
            order = order.ToLower() == "desc" ? "desc" : "asc";

            var query = _appointments.AsQueryable();

            query = sort switch
            {
                "scheduledAt" => order == "asc" ? query.OrderBy(a => a.ScheduledAt) : query.OrderByDescending(a => a.ScheduledAt),
                "status" => order == "asc" ? query.OrderBy(a => a.Status) : query.OrderByDescending(a => a.Status),
                _ => query.OrderBy(a => a.Id)
            };

            var total = query.Count();
            var items = query.Skip((page - 1) * limit).Take(limit).ToList();

            return Ok(new
            {
                data = items,
                meta = new
                {
                    page,
                    limit,
                    total
                }
            });
        }
    }
}