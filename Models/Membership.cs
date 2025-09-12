using System;
using System.ComponentModel.DataAnnotations;
public class Membership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }

    [Required, StringLength(100)]
    public string Plan { get; set; } = string.Empty; // basic | pro | premium

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    [Required, StringLength(10)] // active | expired | canceled
    public string Status { get; set; } = string.Empty;
}

public record CreateMembershipDto
{
    [Required, StringLength(100)]
    public string Plan { get; set; } = string.Empty; // basic | pro | premium
    
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required, StringLength(10)] // active | expired | canceled
    public string Status { get; set; } = string.Empty;
}

public record UpdateMembershipDto
{
    [Required, StringLength(100)]
    public string Plan { get; set; } = string.Empty; // basic | pro | premium

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required, StringLength(10)] // active | expired | canceled
    public string Status { get; set; } = string.Empty;

}