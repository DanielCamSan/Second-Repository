using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckinController : ControllerBase
    {
        private static readonly List<Checkin> _checkins = new()
        {
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.Now() },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-45788", Timestamp = DateTime.UtcNow() },
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-67811", Timestamp = DateTime.UtcNow() }
        };
        

    }
}
/*
 * CreateCheckInDto
UpdateCheckInDto

GET /api/v1/checkins (lista con paginación, orden)

GET /api/v1/checkins/{id}

POST /api/v1/checkins

DELETE /api/v1/checkins/{id}

 */
