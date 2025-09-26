using System;
using System.ComponentModel.DataAnnotations;

public class Pets
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
        
    public string Sex { get; set; } = string.Empty;

    public decimal? WeightKg { get; set; }
}

public record createPetsDTO
{
    [Required]
    public Guid OwnerId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, RegularExpression("^(?i)(dog|cat|bird|reptile|other)$")]
    public string Species { get; set; } = string.Empty;

    [StringLength(100)]
    public string Breed { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required, RegularExpression("^(?i)(male|female|m|f)$")]
    public string sex { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal? WeightKg { get; set; }
}

public record updatePetsDTO
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, RegularExpression("^(?i)(dog|cat|bird|reptile|other)$")]
    public string Species { get; set; } = string.Empty;

    [StringLength(100)]
    public string Breed { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required, RegularExpression("^(?i)(male|female|m|f)$")]
    public string sex { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal? WeightKg { get; set; }
}
/* Guid Id, 
    Guid OwnerId, 
    string Name, 
    string Species // dog | cat | bird | reptile | other 
    string Breed, 
    DateTime BirthDate, 
    string sex, 
    decimal? WeightKg ) 
*/