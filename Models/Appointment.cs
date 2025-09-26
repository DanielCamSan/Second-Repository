using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class Appointment
{
     public Guid Id {  get; set; }
     public Guid PetId { get; set;  }
      [Required]
     public DateTime ScheduledAt { get; set; } = DateTime.Now;
    [Required,StringLength(50)]

    public string Reason { get; set; } = string.Empty;
    [Required,StringLength(50)]

    public string Status { get; set; } = "Scheduled";
    [Required, StringLength(50)]

    public string? Notes { get; set; }


}


public record CreateAppointmentDto
{
    [Required]

    public Guid PetId { get; set; }
    [Required]

    public DateTime ScheduledAt { get; set; }
    [Required, StringLength(50)]
    public string Status { get; set; } = string.Empty;
    [Required, StringLength(50)]

    public string Reason { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string? Notes { get; set; } = string.Empty; 
}

public record UpdateAppointmentDto

{
    [Required]
    public DateTime ScheduledAt { get; set; }
    [Required, StringLength(50)]
    public string Reason { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string Status { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string? Notes { get; set; }

}