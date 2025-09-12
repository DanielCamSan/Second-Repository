using System.ComponentModel.DataAnnotations;
public class Checkin
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    [Range(0, 4000)]
    public DateTime Timestamp { get; set; }
}
public record CreateCheckinDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    [Range(0, 4000)]
    public DateTime Timestamp { get; set; }
}

public record UpdateCheckinDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    [Range(0, 4000)]
    public DateTime Timestamp { get; set; }
}

