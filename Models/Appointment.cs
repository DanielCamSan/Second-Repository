using System.ComponentModel.DataAnnotations;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }

    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "scheduled";
    public string? Notes { get; set; }

    public class CreateAppointmentDto
    {
        [Required]
        public Guid PetId { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        [StringLength(100)]
        public string Reason { get; set; }

        [RegularExpression("scheduled|completed|canceled|no_show")]
        public string Status { get; set; } = "scheduled";

        public string? Notes { get; set; }
    }
    public class UpdateAppointmentDto
    {
        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        [StringLength(100)]
        public string Reason { get; set; }

        [Required]
        [RegularExpression("scheduled|completed|canceled|no_show")]
        public string Status { get; set; }

        public string? Notes { get; set; }
    }
}
