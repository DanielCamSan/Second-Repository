using System.ComponentModel.DataAnnotations;

public class Owner
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 7)]
    public string Phone { get; set; } = string.Empty;

    public bool Active { get; set; } = true; // default requerido
}

// ========== DTOs ==========

public record CreateOwnerDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 7)]
    public string Phone { get; init; } = string.Empty;

    public bool? Active { get; init; } // null => controller lo pone en true
}

public record UpdateOwnerDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 7)]
    public string Phone { get; init; } = string.Empty;

    [Required]
    public bool Active { get; init; } = true;
}
