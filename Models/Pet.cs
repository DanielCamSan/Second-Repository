using System.ComponentModel.DataAnnotations;
//pusiste data anotations innecesarios no pedi que todos sean required!
public class Pet
{
    [Required]
    public Guid Id { get; set; }= Guid.NewGuid();
    [Required]
    public Guid OwnerId { get; set; } = Guid.NewGuid();
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(100)]
    public string Species { get; set; } // dog | cat | bird | reptile | other 
    [Required, StringLength(100)]
    public string Breed { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required, StringLength(20)]
    public string sex { get; set; } // macho | hembra
    [Range(0,500)]
    public decimal? WeightKg { get; set; }
}

public record CreatePetDto
{
    [Required]
    public required Guid OwnerId { get; init; }
    [Required, StringLength(100)]
    public required string Name { get; init; }
    [Required, StringLength(100)]
    public required string Species { get; init; } // dog | cat | bird | reptile | other 
    [Required, StringLength(100)]
    public required string Breed { get; init; }
    [Required]
    public required DateTime BirthDate { get; init; }
    [Required, StringLength(20)]
    public required string sex { get; init; }
    [Range(0, 500)]
    public decimal? WeightKg { get; init; }
}

public record UpdatePetDto
{
    [Required]
    public required Guid OwnerId { get; set; }
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Required, StringLength(100)]
    public required string Species { get; set; } // dog | cat | bird | reptile | other 
    [Required, StringLength(100)]
    public required string Breed { get; set; }
    [Required]
    public required DateTime BirthDate { get; set; }
    [Required, StringLength(20)]
    public required string sex { get; set; }
    [Range(0, 500)]
    public decimal? WeightKg { get; set; }
}
