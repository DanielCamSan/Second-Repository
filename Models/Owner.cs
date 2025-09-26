/*
    Guid Id, 
    string Email, 
    string FullName, 
    bool Active  
 */
using System;
using System.ComponentModel.DataAnnotations;
public class Owner
{
    public Guid Id { get; set; }
    
    [Required, StringLength(100)]
    public string Email { get; set; }
    
    [Required, StringLength(100)]
    public string FullName { get; set; }
    
    public bool Active { get; set; }
    
}

public record CreateOwnerDto
{
    [Required, StringLength(100)]
    public string Email { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; }

    public bool Active { get; set; }
};

public record UpdateOwnerDto
{
    [Required, StringLength(100)]
    public string Email { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; }

    public bool Active { get; set; }
};
