using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Dtos
{
    public class CreateAppointmentDto
    {
        [Required]
        public Guid PetId { get; set; }

        public DateTime? ScheduledAt { get; set; }

        [Required, StringLength(120)]
        public string Reason { get; set; } = string.Empty;

        [RegularExpression("^(scheduled|completed|canceled|no_show)$",
            ErrorMessage = "Status must be one of: scheduled, completed, canceled, no_show")]
        public string? Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
