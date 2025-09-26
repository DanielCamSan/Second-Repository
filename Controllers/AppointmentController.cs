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

        [HttpDelete("{Id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            var count = _appointments.RemoveAll(x => x.Id == id);
            if (count == 0) { return NotFound(new { error = "Appointment Not Found", status = 404 }); }
            else { return NoContent(); }
        }
    }
}
