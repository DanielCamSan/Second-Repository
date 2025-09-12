using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class CheckIn
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string BadgeCode { get; set; } = string.Empty;   // Card

        public DateTime Timestamp { get; set; }

    }

    public record CreateMembershipDTO
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string BadgeCode { get; set; } = string.Empty;   // Card

        public DateTime Timestamp { get; set; }
    }

    public record UpdateMembershipDTO
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string BadgeCode { get; set; } = string.Empty;   // Card

        public DateTime Timestamp { get; set; }
    }
}
