using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class Appointment
{
      public Guid Id {  get; set; }
      public Guid PetId { get; set;  }
      [Required]
      public DateTime ScheduledAt { get; set; }
    [Required,StringLength(50)]

    public string Reason { get; set; } = string.Empty;
    [Required,StringLength(50)]

    public string Status { get; set; } = "Scheduled";
    [Required, StringLength(50)]

    public string? Notes { get; set; }


}


public record CreateAppointmentDto
{
    public DateTime ScheduledAt { get ; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled"; 
    public string? Notes { get; set;  }
}

public record UpdateAppointmentDto
{
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
}