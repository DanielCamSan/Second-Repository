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
    }
}