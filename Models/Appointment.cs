using System;
using System.ComponentModel.DataAnnotations;
public class Appointment
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Reason {  get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
