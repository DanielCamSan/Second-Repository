using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[Controlller]")]
    public class OwnersController : ControllerBase 
    {
        private static readonly List<Owner> _owners = new()
        {
new Owner {Id=Guid.NewGuid(),Email="flora@gmail.com",FullName="Flora Ortiz",Active=true},
new Owner {Id=Guid.NewGuid(),Email="Dabner@gmail.com",FullName="Dabner Orozco",Active=true},
new Owner {Id=Guid.NewGuid(),Email="Andy@gmail.com",FullName="Ricard Andrade",Active=true},

        };
    }
}
