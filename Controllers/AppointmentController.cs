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

        [HttpPost]
        public ActionResult<Appointment> Update(CreateAppointmentDto dto)
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

        [HttpPut("{Id:Guid}/{Id2:Guid}")]
        public ActionResult<Appointment> Update(Guid id, Guid id2, UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
            var index = _appointments.FindIndex(x => x.Id == id && x.PetId==id2);
            if (index == -1) {return NotFound(new { error = "Appointment Not Found", status = 404 }); }
            var update = new Appointment
            {
                Id = id,
                PetId = id2,
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
