using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FirstExam.Dtos;
using FirstExam.Models;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> _db = new()
        {
            new Appointment {
                Id = Guid.NewGuid(),
                PetId = Guid.Parse("00000000-0000-0000-0000-0000000000AA"),
                ScheduledAt = DateTime.UtcNow.AddDays(1),
                Reason = "vacunación", Status = "scheduled", Notes = "Primera dosis"
            },
            new Appointment {
                Id = Guid.NewGuid(),
                PetId = Guid.Parse("00000000-0000-0000-0000-0000000000BB"),
                ScheduledAt = DateTime.UtcNow.AddHours(8),
                Reason = "control", Status = "scheduled"
            }
        };

        // --- GET /api/v1/appointments
        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10,
                                    [FromQuery] string? sort = null, [FromQuery] string? order = "asc")
        {
            Normalize(ref page, ref limit, ref order);

            IEnumerable<Appointment> query = _db;

            query = ApplySort(query, sort, order!);
            var total = query.Count();
            var data = query.Skip((page - 1) * limit).Take(limit).ToList();

            var response = new
            {
                data,
                meta = new { page, limit, total }
            };
            return Ok(response);
        }

        // --- GET /api/v1/appointments/{id} ---
        [HttpGet("{id:guid}")]
        public IActionResult GetOne(Guid id)
        {
            var ap = _db.FirstOrDefault(x => x.Id == id);
            if (ap is null) return NotFound(new { error = "Appointment not found", status = 404 });
            return Ok(ap);
        }

        // --- POST /api/v1/appointments ---
        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var scheduledAt = dto.ScheduledAt?.ToUniversalTime() ?? DateTime.UtcNow;
            var status = string.IsNullOrWhiteSpace(dto.Status) ? "scheduled" : dto.Status!;

            var ap = new Appointment
            {
                Id = Guid.NewGuid(),
                PetId = dto.PetId,
                ScheduledAt = scheduledAt,
                Reason = dto.Reason.Trim(),
                Status = status,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes!.Trim()
            };

            _db.Add(ap);
            return CreatedAtAction(nameof(GetOne), new { id = ap.Id }, ap);
        }

        // --- PUT /api/v1/appointments/{id} ---
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ap = _db.FirstOrDefault(x => x.Id == id);
            if (ap is null) return NotFound(new { error = "Appointment not found", status = 404 });

            if (dto.ScheduledAt.HasValue) ap.ScheduledAt = dto.ScheduledAt.Value.ToUniversalTime();
            if (!string.IsNullOrWhiteSpace(dto.Reason)) ap.Reason = dto.Reason!.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Status)) ap.Status = dto.Status!;
            ap.Notes = dto.Notes?.Trim();

            return Ok(ap);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var ap = _db.FirstOrDefault(x => x.Id == id);
            if (ap is null) return NotFound(new { error = "Appointment not found", status = 404 });

            _db.Remove(ap);
            return NoContent(); 
        }
        private static void Normalize(ref int page, ref int limit, ref string? order)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;
            if (limit > 100) limit = 100;
            order = (order?.ToLowerInvariant() == "desc") ? "desc" : "asc";
        }

        private static IEnumerable<Appointment> ApplySort(IEnumerable<Appointment> src, string? sort, string order)
        {
            Func<Appointment, object> keySelector = sort?.ToLowerInvariant() switch
            {
                "scheduledat" => a => a.ScheduledAt,
                "status" => a => a.Status,
                "reason" => a => a.Reason,
                _ => a => a.Id
            };

            return (order == "desc") ? src.OrderByDescending(keySelector)
                                     : src.OrderBy(keySelector);
        }
    }
}
