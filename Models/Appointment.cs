using FirstExam.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FirstExam.Models
{
    public class Appointment
    {
        [Required] public Guid Id { get; set; }
        [Required] public Guid PetId { get; set; }
        [Required] public DateTime ScheduledAt { get; set; } // fecha y hora agendada  
        [Required] public String Reason { get; set; }// vacunación, control, cirugía, etc. 
        [Required] public String Status { get; set; } = "scheduled";// scheduled | completed | canceled | no_show 
        public String? Notes { get; set; }
    }
    public record CreateAppointmentDto
    {
        [Required] public DateTime ScheduledAt { get; init; } // fecha y hora agendada  
        [Required] public String Reason { get; init; }// vacunación, control, cirugía, etc. 
        [Required] public String Status { get; init; } = "scheduled";// scheduled | completed | canceled | no_show 
        public String? Notes { get; init; }
    }

    public record UpdateAppointmentDto
    {
        [Required] public DateOnly ScheduledAt { get; init; } // fecha y hora agendada  
        [Required] public String Reason { get; init; }// vacunación, control, cirugía, etc. 
        [Required] public String Status { get; init; } = "scheduled";// scheduled | completed | canceled | no_show 
        public String? Notes { get; init; }
    }

}
