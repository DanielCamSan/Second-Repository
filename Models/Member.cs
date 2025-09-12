using System;
using System.ComponentModel.DataAnnotations;

    public class Member
    {
    public Guid Id { get; set; }
    [Required,StringLength(200)]
    public string Email {  get; set; }=string.Empty;
    [Required, StringLength(200)]
    public string FullName {  get; set; }=string.Empty;
    [Required]
    public bool Active { get; set; }

}



