using System.ComponentModel.DataAnnotations;


public class CheckIn
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(15)]
    public string BadgeCode { get; set; } = string.Empty; // código/tarjeta del socio

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public record CreateCheckInDto
{
    [Required]
    public Guid Id { get; init; }

    [Required, StringLength(15)]
    public string BadgeCode { get; init; } = string.Empty;

    [Required]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

public record UpdateCheckInDto
{
    [Required]
    public Guid Id { get; init; }

    [Required, StringLength(15)]
    public string BadgeCode { get; init; } = string.Empty;

    [Required]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}