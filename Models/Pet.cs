using System.ComponentModel.DataAnnotations;
namespace FirstExam.Models
{
    public class Pet
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required,StringLength(100)]
        public string Species { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Breed { get; set; } = string.Empty;
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string sex { get; set; } = string.Empty;
        
        public decimal? WeightKg { get; set; }
    }
}

public record CreatePetDto
{
    [Required,StringLength(100)]
    public required string Name { get; set; }
    [Required, StringLength(100)]
    public required string Species { get; set; }
    [Required, StringLength(100)]
    public required string Breed { get; set; }
    [Required]
    public required DateTime Birthday { get; set; }
    [Required]
    public required string sex { get; set; }

    public decimal? WeightKg { get; set; }
}
public record UpdatePetDto
{
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Required, StringLength(100)]
    public required string Species { get; set; }
    [Required, StringLength(100)]
    public required string Breed { get; set; }
    [Required]
    public required DateTime Birthday { get; set; }
    [Required]
    public required string sex { get; set; }
    public decimal? WeightKg { get; set; }
}