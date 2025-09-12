using System;
using System.ComponentModel.DataAnnotations;
public class Member
{
    public Guid Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    bool Active { get; set; }
}

//DTOs
public record CreateMemberDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    bool Active { get; set; }
}
