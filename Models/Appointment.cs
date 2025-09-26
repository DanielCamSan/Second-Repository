using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PetId { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required, StringLength(120)]
        public string Reason { get; set; } = string.Empty;

        [Required, RegularExpression("^(scheduled|completed|canceled|no_show)$",
            ErrorMessage = "Status must be one of: scheduled, completed, canceled, no_show")]
        public string Status { get; set; } = "scheduled";

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
