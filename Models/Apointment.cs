using System;
using System.ComponentModel.DataAnnotations;

//Nombre del recurso
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
    //tenias que crear en el momento de hacer el appointment
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
    //y el valor por defecto?
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