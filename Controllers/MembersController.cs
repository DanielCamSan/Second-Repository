using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace newCRUD.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "javier@example.com",  FullName = "Javier Fernadez",  Active = true },
            new Member { Id = Guid.NewGuid(), Email = "juan@example.com", FullName = "Juan Gómez", Active = true },
            new Member { Id = Guid.NewGuid(), Email = "luis@example.com", FullName = "Luis Morales", Active = false },
        };

        private static (int page, int limit) NormalizePage(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }
    }
}