using FirstExam.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
//using System.Reflection;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OwnersController:ControllerBase
    {
        private static readonly List<Owner> _owners = new()
        {
            new Owner { Id = Guid.NewGuid() , Email = "maria@gmail.com", FullName = "Maria Lozada"},
            new Owner { Id = Guid.NewGuid() , Email = "juan@gmail.com", FullName = "Juan Perez"},
            new Owner { Id = Guid.NewGuid() , Email = "camilo@gmail.com", FullName = "Camilo Rodriguez"},
            new Owner { Id = Guid.NewGuid() , Email = "victor@gmail.com", FullName = "Victor Medrano"}

        };

        private static (int page, int limit) NormalizaPage(int? page, int? limit)
        {

            var p= page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
            return (p, l);
        }



    }
}
