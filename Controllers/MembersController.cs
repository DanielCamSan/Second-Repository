using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Globalization;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route ("api/[Controller]")]
    public class MembersController
    {
        /*
 GET /api/v1/members (lista con paginación, orden)

GET /api/v1/members/{id}

POST /api/v1/members

PUT /api/v1/members/{id}

DELETE /api/v1/members/{id}
         */

        private static readonly List<Member> _members = new()
        {
            new Member { Id = Guid.NewGuid(), Email = "carlal@gmail.com",FullName="carla lopez",active=true},
            new Member { Id = Guid.NewGuid(), Email = "mat@gamil.com",FullName="matias carballo",active=false},
            new Member { Id = Guid.NewGuid(), Email = "rodrigo@gmail.com",FullName="rodrigo soria",active=true},
            new Member { Id = Guid.NewGuid(), Email = "rosa@gamil.com",FullName="rosaura martinez",active=true}
        };
    }
}
