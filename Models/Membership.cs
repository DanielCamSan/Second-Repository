using System.ComponentModel.DataAnnotations;
public record Membership
{
    public Guid Id { get; set; }
    [Required]
    public Guid MemberId { get; set; }
    [Required, StringLength(50)]
    public string Plan {  get; set; } // basic | pro | premium
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required, StringLength(50)]
    public string Status { get; set; }// active | expired | canceled
}
public record CreateMembershipDto
{
    [Required]
    public Guid MemberId { get; init; }
    [Required, StringLength(50)]
    public string Plan { get; init; }
    [Required]
    public DateTime StartDate { get; init; }
    [Required]
    public DateTime EndDate { get; init; }
    [Required, StringLength(50)]
    public string Status { get; init; }
}
public record UpdateMembershipDto
{
    [Required]
    public Guid MemberId { get; init; }
    [Required, StringLength(50)]
    public string Plan { get; init; }
    [Required]
    public DateTime StartDate { get; init; }
    [Required]
    public DateTime EndDate { get; init; }
    [Required, StringLength(50)]
    public string Status { get; init; }
}