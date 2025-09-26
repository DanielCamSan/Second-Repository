using System;
using System.ComponentModel.DataAnnotations;

    public class Owner
    {
    public Guid Id { get; set; }
    public string FullName { get; set; }=string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool Active { get; set; } 
}
//DTO 
public record CreateOwnerDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

}
public record UpdateOwnerDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}