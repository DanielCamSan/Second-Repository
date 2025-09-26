using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class OwnersController : ControllerBase
{
    private static (int page, int limit, string order) Normalize(int? page, int? limit, string? order)
    {
        var p = page.GetValueOrDefault(1); if (p < 1) p = 1;
        var l = limit.GetValueOrDefault(10); if (l < 1) l = 1; if (l > 100) l = 100;
        var o = (order?.ToLower() is "asc" or "desc") ? order!.ToLower() : "asc";
        return (p, l, o);
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? limit,
                                [FromQuery] string? sort, [FromQuery] string? order)
    {
        var (p, l, o) = Normalize(page, limit, order);
        IEnumerable<Owner> query = InMemoryDb.Owners;

        // Ordenamiento seguro con whitelist
        query = (sort?.ToLower()) switch
        {
            "fullname" => (o == "desc") ? query.OrderByDescending(x => x.FullName) : query.OrderBy(x => x.FullName),
            "email" => (o == "desc") ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
            "phone" => (o == "desc") ? query.OrderByDescending(x => x.Phone) : query.OrderBy(x => x.Phone),
            "active" => (o == "desc") ? query.OrderByDescending(x => x.Active) : query.OrderBy(x => x.Active),
            _ => (o == "desc") ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
        };

        var total = query.Count();
        var items = query.Skip((p - 1) * l).Take(l).ToList();
        return Ok(new { data = items, meta = new { page = p, limit = l, total } });
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetOne(Guid id)
    {
        var item = InMemoryDb.Owners.FirstOrDefault(x => x.Id == id);
        return item is null ? NotFound(new { error = "Owner not found", status = 404 }) : Ok(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOwnerDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = new Owner
        {
            Email = dto.Email.Trim(),
            FullName = dto.FullName.Trim(),
            Phone = dto.Phone.Trim(),
            Active = dto.Active ?? true
        };

        InMemoryDb.Owners.Add(entity);
        return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateOwnerDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var idx = InMemoryDb.Owners.FindIndex(x => x.Id == id);
        if (idx == -1) return NotFound(new { error = "Owner not found", status = 404 });

        InMemoryDb.Owners[idx] = new Owner
        {
            Id = id,
            Email = dto.Email.Trim(),
            FullName = dto.FullName.Trim(),
            Phone = dto.Phone.Trim(),
            Active = dto.Active
        };
        return Ok(InMemoryDb.Owners[idx]);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var removed = InMemoryDb.Owners.RemoveAll(x => x.Id == id);
        return removed == 0 ? NotFound(new { error = "Owner not found", status = 404 }) : NoContent();
    }
}
