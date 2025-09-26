using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> _appointments = new()
        {
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "scheduled", Reason = "control" },
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "completed", Reason = "control" },
            new Appointment {Id = Guid.NewGuid(), PetId = Guid.NewGuid(), ScheduledAt = DateTime.Now, Status = "canceled", Reason = "vacunacion", Notes = "Alergia" }
        };

    }
}
