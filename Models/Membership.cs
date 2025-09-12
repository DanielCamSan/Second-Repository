using System;
using System.ComponentModel.DataAnnotations;

public class Membership
{
    public Guid Id { get; set; }

    [Required]
    public Guid MemberId { get; set; }

    [StringLength(10)]
    [RegularExpression("^(basic|pro|premium)$")]
    public string Plan { get; set; } = string.Empty;
   
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [StringLength(10)]
    [RegularExpression("^(active|expired|canceled)$")]
    public string Status { get; set; } = string.Empty;
}