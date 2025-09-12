using System.ComponentModel.DataAnnotations;

public class Member
{
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Email { get; set; }

    [Required, StringLength(200)]
    public string FullName { get; set; }

    public bool Active{ get; set; }

}

public record CreateMemberDto
{
    [Required, StringLength(200)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(200)]
    public string FullName { get; init; } = string.Empty;   

    public bool Active { get; init; } = false;
}

public record UpdateMemberDto
{
    [Required, StringLength(200)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(200)]
    public string FullName { get; init; } = string.Empty;

    public bool Active { get; init; } = false;
}