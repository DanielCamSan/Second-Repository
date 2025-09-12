using System.ComponentModel.DataAnnotations;
using System;
public class Member
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public bool active { get; set; } 
}
/*
  Guid Id,
  string Email,
  string FullName,
  bool Active
  */

//DTOs
//CreateMemberDto
public record CreateMemberDto
{
    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public bool active { get; set; }
}
//UpdateMemberDto

public record UpdateMemberDto
{
    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public bool active { get; set; }
}