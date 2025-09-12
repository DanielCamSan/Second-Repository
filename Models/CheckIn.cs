using System;
using System.ComponentModel.DataAnnotations;
public class CheckIn
{

    public Guid id { get; set; }

    [Required, StringLength(8)]
    public string BadgeCode { get; set; } = string.Empty;

    [Required]
    public DateTime Timestamp;
}

public record CreateCheckinDto
{
    [Required, StringLength(8)]
    public string BadgeCode { get; set; } = string.Empty;

    [Required]
    public DateTime Timestamp;
}
public record UpdateCheckinDto
{
    [Required, StringLength(8)]
    public string BadgeCode { get; set; } = string.Empty;

    [Required]
    public DateTime Timestamp;
}