using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    public class MembersController
    {
        [ApiController]
        [Route("api/[controller]")]

        public class MembersController : ControllerBase
        {
            private static List<Member> members = new List<Member>
            {
                new Member { Id = 1, Name = "John Doe", Email = ""}

            };


        
        
        }
}
