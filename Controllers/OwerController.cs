using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Controllers
{
    //mal nombre del controller
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class OwerController : Controller
    {
        //Inicializar la lista la siguiente vez es mas dificil probar
        private static readonly List<Owners> _owners = new();

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? sort = "Email",
            [FromQuery] string order = "asc")
        { 
            if(page< 1){
                page = 1;
            }
            if (limit < 10) {
                limit = 10;
            }
            if(limit > 100)
            {
                limit = 100;
            }
            if(order != "asc" && order != "desc")
            {
                order = "asc";
            }
            var query = _owners.AsQueryable();
            //no funciona para nada mas que Email
            query = sort switch
            {
                "Email" => order == "asc" ? query.OrderBy(a => a.Email) : query.OrderByDescending(a => a.Email)
            };
            var total = query.Count();
            var items = query.Skip((page-1)*limit).Take(total).ToList();
            return Ok(new { data = items, meta = new { page, limit, total } });

        }
        [HttpGet("{id:guid}")]
        public ActionResult GetById(Guid id)
        {
            var item = _owners.FirstOrDefault(o => o.Id == id);
            return item == null ? NotFound() : View(item);
        }
        //Los creas vacios y no los devuelves
        [HttpPost]
        public IActionResult Create([FromBody] CreateOwerDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var item = new Owners { Id = dto.Id, Active = true };
            _owners.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item }, item);
        }
        //Mala ruta 
        [HttpDelete]
        public IActionResult DeleteById(Guid id)
        {
            var item = _owners.FirstOrDefault(a => a.Id == id);

            if (item == null) return NotFound();
            _owners.Remove(item);
            return NoContent();
        }
    }
}
