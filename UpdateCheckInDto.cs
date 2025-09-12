using System.ComponentModel.DataAnnotations;

public record UpdateCheckInDto(
    [Required, StringLength(20)] string BadgeCode
);
