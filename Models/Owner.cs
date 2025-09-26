using System;
using System.ComponentModel.DataAnnotations;

public class Owner
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }
    [Required, StringLength(50)]
    public string FullName { get; set; }
  
    public bool Active {  get; set; } =true;
}
public record CreateOwnerDto
{
 

    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}

public record UpdateOwnerDto
{


    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}