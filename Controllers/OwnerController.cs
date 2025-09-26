using Microsoft.AspNetCore.Mvc;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OwnerController : ControllerBase
    {
        private static readonly List<Owner> Owners = new()
        {
            new Owner { Id = Guid.NewGuid(),Email = "JuanPerez@gmail.com", FullName = "Juan Perez", Active = true },
            new Owner { Id = Guid.NewGuid(),Email = "MariaGomez@gmail.com", FullName = "Maria Gomez", Active = true },
            new Owner { Id = Guid.NewGuid(),Email = "CarlosSanchez@gmail.com", FullName = "Carlos Sanchez", Active = false }
        };
    }
}
