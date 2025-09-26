using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
public class Owner
{
    [Required]
    public Guid Id { get; set; }

    [Required, Length(0, 100)]
    public string Email { get; set; } = "";

    [Required, Length(0, 100)]
    public string FullName { get; set; } = "";

    [Required]

    public bool Active { get; set; }
}
public record CreateOwnerDto
{
    [Required]
     public Guid Id { get; init; }

    [Required, Length(0, 100)]
     public string Email { get; init; } = "";

    [Required, Length(0, 100)]
     public string FullName { get; init; } = "";

    [Required]

     public bool Active { get; init; }
}
public record UpdateOwnerDto
{
    [Required]
     public Guid Id { get; init; }

    [Required, Length(0, 100)]
     public string Email { get; init; } = "";

    [Required, Length(0, 100)]
     public string FullName { get; init; } = "";

    [Required]

     public bool Active { get; init; }
}
