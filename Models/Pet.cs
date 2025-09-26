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
