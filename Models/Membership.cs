using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Membership
    {
        public Guid Id { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        [StringLength(50)]
        public string Plan { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "active";
    }
    public record CreateMembershipDto
    {
        [Required(ErrorMessage = "El ID del miembro es obligatorio.")]
        public Guid MemberId { get; init; }

        [Required(ErrorMessage = "El plan es obligatorio.")]
        [RegularExpression("^(basic|pro|premium)$", ErrorMessage = "El plan debe ser 'basic', 'pro' o 'premium'.")]
        public string Plan { get; init; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; init; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime EndDate { get; init; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("^(active|expired|canceled)$", ErrorMessage = "El estado debe ser 'active', 'expired' o 'canceled'.")]
        public string Status { get; init; } = "active";
    }

    public record UpdateMembershipDto
    {
        [Required(ErrorMessage = "El plan es obligatorio.")]
        [RegularExpression("^(basic|pro|premium)$", ErrorMessage = "El plan debe ser 'basic', 'pro' o 'premium'.")]
        public string Plan { get; init; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; init; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime EndDate { get; init; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("^(active|expired|canceled)$", ErrorMessage = "El estado debe ser 'active', 'expired' o 'canceled'.")]
        public string Status { get; init; } = string.Empty;
    }
}