using System;
using System.ComponentModel.DataAnnotations;

public class Membership
{
    public Guid Id { get; set; }

    [Required]
    public Guid MemberId { get; set; }

    [Required, StringLength(10)]
    [RegularExpression("^(basic|pro|premium)$")]
    public string Plan { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    [Required, StringLength(10)]
    [RegularExpression("^(active|expired|canceled)$")]
    public string Status { get; set; } = string.Empty;
}

//DTOs (input/output for the API)
public record CreateMembershipDto
{
    [Required]
    public Guid MemberId { get; set; }

    [Required, StringLength(10)]
    [RegularExpression("^(basic|pro|premium)$")]
    public string Plan { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required, StringLength(10)]
    [RegularExpression("^(active|expired|canceled)$")]
    public string Status { get; set; } = string.Empty;
}