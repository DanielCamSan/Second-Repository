using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Controllers
{
    public class AppointmentController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
