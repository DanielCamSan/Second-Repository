using System;
using System.ComponentModel.DataAnnotations;

public class Apointment
{
    public Guid Id { get; set; }
    [Required]
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }
}


public record CreateApointmentDto
{
    [Required]
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }
}

public record UpdateApointmentDto
{
    [Required]
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }
}