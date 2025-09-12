using System.ComponentModel.DataAnnotations;

public record CreateCheckInDto(
    [Required, StringLength(20)] string BadgeCode
);
