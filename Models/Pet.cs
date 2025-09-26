using System;
using System.ComponentModel.DataAnnotations;
public class Pet
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Species { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Breed { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required, StringLength(100)]
    public string Sex { get; set; } = string.Empty;

    [Required]
    public decimal? WeightKg { get; set; }

}

public record CreatePetDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Species { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Breed { get; set; } = string.Empty;
    [Required]
    public DateTime BirthDate { get; set; }
    [Required, StringLength(100)]
    public string Sex { get; set; } = string.Empty;
    public decimal? WeightKg { get; set; }
}

public record UpdatePetDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Species { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Breed { get; set; } = string.Empty;
    [Required]
    public DateTime BirthDate { get; set; }
    [Required, StringLength(100)]
    public string Sex { get; set; } = string.Empty;
    public decimal? WeightKg { get; set; }
}