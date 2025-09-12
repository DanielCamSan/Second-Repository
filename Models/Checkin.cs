using System;
using System.ComponentModel.DataAnnotations;
public class Checkin
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    public  DateTime Timestamp{ get; set; } 
}
// first DTO create 
public record CreateCheckInDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; init; } = string.Empty;
}
// Second DTO update
public record UpdateCheckInDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; init; } = string.Empty;
}