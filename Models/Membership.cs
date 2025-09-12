using System;
using System.ComponentModel.DataAnnotations;
public class Membership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }

    public string Plan { get; set; } = string.Empty;
    public DateTime StartDate = DateTime.Now;
    public DateTime EndDate = DateTime.Now;
    public string Status { get; set; } = string.Empty;
}


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

