using System.ComponentModel.DataAnnotations;
namespace FirstExam.Models
{
    public class Pet
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        [Required]
        public string sex { get; set; } = string.Empty;
        
        public decimal? WeightKg { get; set; }
    }
}

public record CreatePetDto
{
    public required string Name { get; set; }
    [Required]
    public required string Species { get; set; }
    
    public required string Breed { get; set; }
    
    public required DateTime Birthday { get; set; }
    [Required]
    public required string sex { get; set; }

    public decimal? WeightKg { get; set; }
}
public record UpdatePetDto
{
    public required string Name { get; set; }
    [Required, StringLength(100)]
    public required string Species { get; set; }
    public required string Breed { get; set; }
    public required DateTime Birthday { get; set; }
    [Required]
    public required string sex { get; set; }
    public decimal? WeightKg { get; set; }
}