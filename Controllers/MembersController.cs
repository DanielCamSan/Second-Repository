using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "j.deer@gmail.com" , FullName = "John Deer", Active = true}

        };
    }
}
