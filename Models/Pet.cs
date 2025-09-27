using System;
using System.ComponentModel.DataAnnotations;
public class Pet
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
        
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(50), AllowedValues("dog", "cat", "bird", "reptile", "other")]
    public string Species { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    [Required, StringLength(50), AllowedValues("male", "female")]
    public string Sex { get; set; } = string.Empty;
    [Range(0,100)]
    public decimal? WeightKg { get; set; }
}

//DTOs

//mal nombre de los dtos, NO ME COMPILA

public record CreatePetDto
//public record CreateVetDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(50)]
    public string Species { get; set; }
    [Required, StringLength(50)]
    public string Sex { get; set; }
}

public record UpdatePetDto
//public record UpdateVetDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(50)]
    public string Species { get; set; }
    [Required, StringLength(50)]
    public string Sex { get; set; }
}