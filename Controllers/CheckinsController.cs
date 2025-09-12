using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class CheckinsController : ControllerBase
    {
        private static readonly List<Checkin> chechins = new()
        {
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-12345", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-678910", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-111213", Timestamp = DateTime.Now},
            new Checkin { Id = Guid.NewGuid(), BadgeCode = "GYM-141516", Timestamp = DateTime.Now},
        };
        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
    }
}