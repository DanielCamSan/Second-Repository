using System;
using System.ComponentModel.DataAnnotations;

public class CheckIn
{
    public Guid Id { get; set; }
    [Required, StringLength(100)]

    public string BadgeCode { get; set; } = string.Empty;
    [Required, StringLength(100)]

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

    // DTOs
    public record CreateCheckInDto
    {
        [Required, StringLength(40)]
        [RegularExpression(@"\S(?:.*\S)?", ErrorMessage = "BadgeCode cannot be empty or only whitespace.")]
        public string BadgeCode { get; init; } = string.Empty;
       
    }

    public record UpdateCheckInDto
    {
        [Required, StringLength(40)]
        [RegularExpression(@"\S(?:.*\S)?", ErrorMessage = "BadgeCode cannot be empty or only whitespace.")]
        public string BadgeCode { get; init; } = string.Empty;

        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }
