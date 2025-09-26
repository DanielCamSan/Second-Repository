using System;
using System.ComponentModel.DataAnnotations;

public class Pet
{
    public Guid Guid { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Sex { get; set; } = string.Empty;
    public decimal? WeightKg { get; set; }
}
public record CreatePetDto
{
    public Guid OwnerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Species { get; init; } = string.Empty;
    public string Breed { get; init; } = string.Empty;
    public DateTime Birthdate { get; init; }
    public string Sex { get; init; } = string.Empty;
    public decimal? WeightKg { get; init; }
}