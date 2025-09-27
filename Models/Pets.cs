using System;
using System.ComponentModel.DataAnnotations;

public class Pets
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Species { get; set; } = string.Empty;


    [Required, StringLength(100)]
    public string Breed { get; set; } = string.Empty;

    //Era birthdate
    public DateTime BirthData { get; set; }

    [Required, StringLength(100)]
    public string Sex { get; set; } = string.Empty;

    [Required,Range(0,80)]
    public decimal WeightKg { get; set; }

}


public record CreatePetDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }

    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Species { get; init; } = string.Empty;


    [Required, StringLength(100)]
    public string Breed { get; init; } = string.Empty;


    public DateTime BirthData { get; init; }

    [Required, StringLength(100)]
    public string Sex { get; init; } = string.Empty;

    [Required, Range(0, 80)]
    public decimal WeightKg { get; init; }
}

public record UpdatePetDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }

    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Species { get; init; } = string.Empty;


    [Required, StringLength(100)]
    public string Breed { get; init; } = string.Empty;


    public DateTime BirthData { get; init; }

    [Required, StringLength(100)]
    public string Sex { get; init; } = string.Empty;

    [Required, Range(0, 80)]
    public decimal WeightKg { get; init; }
}
