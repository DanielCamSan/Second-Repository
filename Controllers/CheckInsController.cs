using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class CheckInsController : ControllerBase
{
    private static List<CheckIn> checkIns = new();
    [HttpGet]
    public IActionResult GetAll(int page = 1, int limit = 10, string? sort = "timestamp", string order = "asc")
    {
        if (page < 1) page = 1;
        if (limit < 1 || limit > 100) limit = 10;

        IEnumerable<CheckIn> query = checkIns;

        query = sort?.ToLower() switch
        {
            "badgecode" => (order == "desc") ? query.OrderByDescending(c => c.BadgeCode) : query.OrderBy(c => c.BadgeCode),
            "timestamp" => (order == "desc") ? query.OrderByDescending(c => c.Timestamp) : query.OrderBy(c => c.Timestamp),
            _ => query.OrderBy(c => c.Timestamp)
        };

        var total = query.Count();
        var data = query.Skip((page - 1) * limit).Take(limit).ToList();

        return Ok(new { data, meta = new { page, limit, total } });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var checkIn = checkIns.FirstOrDefault(c => c.Id == id);
        if (checkIn == null)
            return NotFound(new { error = "CheckIn not found", status = 404 });

        return Ok(checkIn);
    }


    [HttpPost]
    public IActionResult Create(CreateCheckInDto dto)
    {
        var checkIn = new CheckIn
        {
            Id = Guid.NewGuid(),
            BadgeCode = dto.BadgeCode,
            Timestamp = DateTime.UtcNow
        };

        checkIns.Add(checkIn);

        return CreatedAtAction(nameof(GetById), new { id = checkIn.Id }, checkIn);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UpdateCheckInDto dto)
    {
        var checkIn = checkIns.FirstOrDefault(c => c.Id == id);
        if (checkIn == null)
            return NotFound(new { error = "CheckIn not found", status = 404 });

        checkIn.BadgeCode = dto.BadgeCode;
        return Ok(checkIn);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var checkIn = checkIns.FirstOrDefault(c => c.Id == id);
        if (checkIn == null)
            return NotFound(new { error = "CheckIn not found", status = 404 });

        checkIns.Remove(checkIn);
        return NoContent();
    }
}
