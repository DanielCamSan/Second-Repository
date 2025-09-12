
using System;
using System.ComponentModel.DataAnnotations;


public class Member
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class CreateMemberDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(150)]
    public string FullName { get; init; } = string.Empty;

    public bool Active { get; init; }
}

public class UpdateMemberDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(150)]
    public string FullName { get; init; } = string.Empty;

    public bool Active { get; init; }
}

