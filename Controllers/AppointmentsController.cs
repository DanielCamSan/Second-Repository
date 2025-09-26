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
    }
}