using System;
using System.ComponentModel.DataAnnotations;
public class Membership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    [Required, StringLength(10)]
    public string Plan { get; set; } = string.Empty;
    public DateTime StartDate = DateTime.Now;
    public DateTime EndDate = DateTime.Now;
    [Required, StringLength(15)]
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

