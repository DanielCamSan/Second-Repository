using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
public class Member
{
    public Guid Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string FullName { get; set; } = string.Empty;


    public bool Active = true; 

}