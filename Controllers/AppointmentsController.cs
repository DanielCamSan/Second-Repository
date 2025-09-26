using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Intrinsics.X86;



namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        public static readonly List<Appointments> _appointments = new()
        {
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="control",Status="completed"},
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="vaccination",Status="scheduled", Notes="ok"},
           new Appointments {Id=Guid.NewGuid(),PetId=Guid.NewGuid(),Schedule=DateTime.Now,Reason="cirugise",Status="canceled", Notes="hard"}
        };
         
        private static (int page, int limit) NormalizePagination(int? page, int? limit)
        {
            var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
            var l = limit.GetValueOrDefault(10); if (l < 100) l = 100;
            return (p, l);
        }

        private static IEnumerable<T> OrderByProp<T>(IEnumerable<T> src, string? sort, string? order)
        {
            if (string.IsNullOrWhiteSpace(sort)) return src;
            var prop = typeof(T).GetProperty(sort,BindingFlags.IgnoreCase|BindingFlags.Public|BindingFlags.Instance);
            if (prop is null) return src;
            return string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                ? src.OrderByDescending(x=>prop.GetValue(x))
                : src.OrderBy(x=>prop.GetValue(x));
        }

        [HttpGet]

        public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? limit,[FromQuery] string? sort, [FromQuery] string? order, [FromQuery] string? q, [FromQuery] string? name)
        {
            var (p,l)=NormalizePagination(page, limit);
            IEnumerable<Appointments> query = _appointments;
            if(!string.IsNullOrWhiteSpace(q))
                query=query.Where(m=>m.Reason.Contains(q, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Reason.Contains(name, StringComparison.OrdinalIgnoreCase));
            query=OrderByProp(query, order, sort);
            var total =query.Count();
            var items=query.Skip((p-1)*1).Take(total).ToList();
            return Ok(new { total, page = p, limit = l, items });
        }

        [HttpGet ("{id:Guid}")]

        public IActionResult GetOne (Guid id)
        {
            var appointment = _appointments.FirstOrDefault(a=> a.Id == id);
            return appointment is null
                ? NotFound(new { error = "No se encontro", status = 404 })
                : Ok(appointment);
        }

        [HttpPost]
        public ActionResult<Appointments> Create([FromBody] CreateAppointmentsDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var appointments = new Appointments
            {
                Id = Guid.NewGuid(),
                PetId = Guid.NewGuid(),
                Schedule= DateTime.Now,
                Reason=dto.Reason,
                Status=dto.Status,
                Notes=dto.Notes
               
            };

            _appointments.Add(appointments);
            return CreatedAtAction(nameof(GetOne), new { id = appointments.Id }, appointments);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Appointments> Update(Guid id, [FromBody] UpdateAppointmentsDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var index = _appointments.FindIndex(a => a.Id == id);
            if (index == -1)
                return NotFound(new { error = "Subscription not found", status = 404 });

            var updated = new Appointments
            {
                Id = Guid.NewGuid(),
                PetId = Guid.NewGuid(),
                Schedule = DateTime.Now,
                Reason = dto.Reason,
                Status = dto.Status,
                Notes = dto.Notes

            };

            _appointments[index] = updated;
            return Ok(updated);
        }




    }
}
