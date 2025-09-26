using System;
using System.ComponentModel.DataAnnotations;

    public class Owner
    {
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
    }
//DTO 
