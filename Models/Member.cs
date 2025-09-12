
using System;
using System.ComponentModel.DataAnnotations;
public class Member
    //Guid Id,
    //string Email,
    //string FullName,
    //bool Active
{
    public Guid Id { get; set; }
    [Required, StringLength(100)] 
    public string Email { get; set; }=string.Empty;
    [Required, StringLength(100)]
    public string FullName { get; set; }= string.Empty;

    public bool Active { get; set; } = true;

}


// DTOs (entrada/salida para la API)
public record CreateUserDto
{
    [Required, StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string FullName { get; init; } = string.Empty;
    public bool Active { get; set; } = true;
}

public record UpdateUserDto
{
    [Required, StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string FullName { get; init; } = string.Empty;

    public bool Active { get; set; } = true;
}


