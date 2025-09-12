using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FirstExam.Models;
using FirstExam.DTOs;

namespace FirstExam.Controllers
{
    [ApiController]
    [Route("api/v1/checkins")]
    public class CheckInsController : ControllerBase
    {
        private static List<CheckIn> _checkIns = new List<CheckIn>();

        // GET: api/v1/checkins
        [HttpGet]
        public IActionResult GetCheckIns([FromQuery] int page = 1, [FromQuery] int limit = 10,
                                        [FromQuery] string sort = "Timestamp", [FromQuery] string order = "asc")
        {
            // Validar parámetros
            if (page < 1) page = 1;
            if (limit < 1 || limit > 100) limit = 10;

            // Ordenamiento seguro
            IQueryable<CheckIn> query = _checkIns.AsQueryable();

            switch (sort.ToLower())
            {
                case "badgecode":
                    query = (order.ToLower() == "desc") ?
                        query.OrderByDescending(c => c.BadgeCode) :
                        query.OrderBy(c => c.BadgeCode);
                    break;
                default: // "timestamp" por defecto
                    query = (order.ToLower() == "desc") ?
                        query.OrderByDescending(c => c.Timestamp) :
                        query.OrderBy(c => c.Timestamp);
                    break;
            }

            // Paginación
            var total = query.Count();
            var checkIns = query.Skip((page - 1) * limit).Take(limit).ToList();

            // Respuesta estándar
            return Ok(new
            {
                data = checkIns,
                meta = new { page, limit, total }
            });
        }

        // GET: api/v1/checkins/{id}
        [HttpGet("{id}")]
        public IActionResult GetCheckIn(Guid id)
        {
            var checkIn = _checkIns.FirstOrDefault(c => c.Id == id);
            if (checkIn == null)
            {
                return NotFound(new { error = "CheckIn not found", status = 404 });
            }
            return Ok(checkIn);
        }

        // POST: api/v1/checkins
        [HttpPost]
        public IActionResult CreateCheckIn([FromBody] CreateCheckInDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var checkIn = new CheckIn
            {
                Id = Guid.NewGuid(),
                BadgeCode = dto.BadgeCode,
                Timestamp = DateTime.UtcNow
            };

            _checkIns.Add(checkIn);

            return CreatedAtAction(nameof(GetCheckIn), new { id = checkIn.Id }, checkIn);
        }

        // DELETE: api/v1/checkins/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCheckIn(Guid id)
        {
            var checkIn = _checkIns.FirstOrDefault(c => c.Id == id);
            if (checkIn == null)
            {
                return NotFound(new { error = "CheckIn not found", status = 404 });
            }

            _checkIns.Remove(checkIn);
            return NoContent();
        }
    }
}