using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;



namespace FirstExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        public static readonly List<Appointments> _appointments = new()
        {
         //  new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason=},
        };

    }
}
