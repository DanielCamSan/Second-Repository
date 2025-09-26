using System;
using System.ComponentModel.DataAnnotations;
    public class Appointments
    {
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public DateTime Schedule {  get; set; }

    [Required , StringLength(100)]
    public string Reason { get; set; } = string.Empty;

    [Required, StringLength(100)]

    public string Status { get; set; } = string.Empty ;

    [Required, StringLength(100)]
    public string? Notes {  get; set; } = string.Empty ;
 

    }

public record CreateAppointmentsDto
{
        
    public Guid Id { get; init; }
    public Guid PetId { get; init; }
    public DateTime Schedule { get; init; }

    [Required, StringLength(100)]
    public string Reason { get; init; } = string.Empty;

    [Required, StringLength(100)]

    public string Status { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string? Notes { get; init; } = string.Empty;

}



public record UpdateAppointmentsDto
{

    public Guid Id { get; init; }
    public Guid PetId { get; init; }
    public DateTime Schedule { get; init; }

    [Required, StringLength(100)]
    public string Reason { get; init; } = string.Empty;

    [Required, StringLength(100)]

    public string Status { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string? Notes { get; init; } = string.Empty;

}