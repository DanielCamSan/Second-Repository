using System;
using System.ComponentModel.DataAnnotations;
public class CheckIn
{
    public Guid Id { get; set; }
    [Required, StringLength(100)]
    public string BadgeCode { get; set; }    // código/tarjeta del socio (ej: "GYM-12345")
    [Required]
    public DateTime Timestamp { get; set; }
}

public record CreateCheckInDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; init; } = string.Empty;
    [Required]
    public DateTime Timestamp { get; init; } = DateTime.Now;
}

public record UpdateCheckInDto 
{
    [Required, StringLength(100)]
    public string BadgeCode { get; init; } = string.Empty;
    [Required]
    public DateTime Timestamp { get; init; } = DateTime.Now;
}
