namespace FirstExam.Models;
using System;
using System.ComponentModel.DataAnnotations;


public class Owner
{
    public Guid Id { get; set; }


    [Required, StringLength(100)]
    public string Email { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; }

    public bool Active { get; set; } = true;

}

public record CreateOwnerDto
{
    [Required, StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; init; } = string.Empty;
    
    public bool Active { get; set; } = true;
}

public record UpdateOwnerDto
{
    [Required, StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string FullName { get; init; } = string.Empty;

    public bool Active { get; set; } = true;

}

