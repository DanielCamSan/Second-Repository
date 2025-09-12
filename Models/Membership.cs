using System.ComponentModel.DataAnnotations;
using System;
public class Membership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }

    [Required, StringLength(10)]
    public string Plan { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required, StringLength(10)]
    public string Status { get; set; } = string.Empty;
}

//DTO's 
public record CreateMembershipDto
{
    [Required, StringLength(10)]
    public string Plan { get; init; } = string.Empty;

    [Required]
    public DateTime StartDate { get; init; }

    [Required]
    public DateTime EndDate { get; init; }

    [Required, StringLength(10)]
    public string Status { get; init; } = string.Empty;
}
public record UpdateMembershipDto
{
    [Required, StringLength(10)]
    public string Plan { get; init; } = string.Empty;

    [Required]
    public DateTime StartDate { get; init; }

    [Required]
    public DateTime EndDate { get; init; }

    [Required, StringLength(10)]
    public string Status { get; init; } = string.Empty;
}
/*
Membership(
    Guid Id,
    Guid MemberId,
    string Plan,         // basic | pro | premium
    DateTime StartDate,
    DateTime EndDate,
    string Status        // active | expired | canceled
)*/