using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
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
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;

        [RegularExpression("^(dog | cat | bird | reptile | other)$")]
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }


        [RegularExpression("^(M | F )$")]
        public string sex { get; set; } = string.Empty;

        public decimal? WeightKg { get; set; }
    }
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