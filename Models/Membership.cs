using System;
using System.ComponentModel.DataAnnotations;
public class Membership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }

    public string Plan { get; set; } = string.Empty;
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    string Status { get; set; } = string.Empty;
}

// DTOs (entrada/salida para la API)
public record CreateMembershipDto
{
    public Guid MemberId { get; init; }
    public string Plan { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
}
public record UpdateMembershipDto
{
    public Guid MemberId { get; init; }
    public string Plan { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
}

