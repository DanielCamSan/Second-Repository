using System;
using System.ComponentModel.DataAnnotations;

public class Member
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }= string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    public bool Active { get; set; } = true;

}
//DTOs for creating and updating members

public class CreateMemberDto
{
    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;
}

public class UpdateMemberDto
{
    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}