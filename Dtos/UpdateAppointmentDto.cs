using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Dtos
{
    public class UpdateAppointmentDto
    {
        public DateTime? ScheduledAt { get; set; }

        [StringLength(120)]
        public string? Reason { get; set; }

        [RegularExpression("^(scheduled|completed|canceled|no_show)$",
            ErrorMessage = "Status must be one of: scheduled, completed, canceled, no_show")]
        public string? Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
