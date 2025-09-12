using System;
using System.ComponentModel.DataAnnotations;

public class CheckIn
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string BadgedCode { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public DateTime TimesStap { get; set; } = DateTime.Now;
}

public record CreateCheckInDto
{
    [Required, StringLength(100)]
    public string BadgedCode { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public DateTime TimesStap { get; set;} = DateTime.Now;
    
}
public record UpdateCheckInDto
{
    [Required, StringLength(100)]
    public string BadgedCode { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public DateTime TimesStap { get; set; } = DateTime.Now;

}
