using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PetsController : ControllerBase
    {
        private static readonly List<Pet> _pets = new()
        {
            new Pet {Id = Guid.NewGuid(), Name = "Rocky", Species = "dog", Sex = "male" },
            new Pet {Id = Guid.NewGuid(), Name = "Cookie", Species = "cat", Sex = "female" },
            new Pet {Id = Guid.NewGuid(), Name = "Red", Species = "bird", Sex = "male" },
        };


    }
}

