using System;
using System.ComponentModel.DataAnnotations;
public class Appointment
{
    public Guid Id { get; set; }
    [Required]
    public Guid PetId { get; set; }
    public DateTime ScheduledAt { get; set; }
    [Required, StringLength(200)]
    public string Reason {  get; set; } = string.Empty;
    [Required]
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public record CreateAppointmentDto
{
    [Required]
    public Guid PetId { get; set; }
    [Required, StringLength(200)]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}