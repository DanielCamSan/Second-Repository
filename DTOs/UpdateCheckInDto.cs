using System.ComponentModel.DataAnnotations;

namespace FirstExam.DTOs
{
    public class UpdateCheckInDto
    {
        [Required(ErrorMessage = "BadgeCode is required")]
        public string BadgeCode { get; set; } = string.Empty;
    }
}
