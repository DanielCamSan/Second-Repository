using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Pet
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid ownerId { get; set; }= Guid.NewGuid();

        public string name { get; set; }

        [Required]
        public string species { get; set; }


        public string breed { get; set; }

        public DateTime birthDate{ get; set; }= DateTime.Now;   


        public string sex { get; set; }

        [Required]
        public decimal? weightKg { get; set; }

    }

    public record CreatePetDto
    {
        [Required]
        public Guid ownerId { get; set; }

        public string name { get; set; }


        public string species { get; set; }


        public string breed { get; set; }

        public DateTime birthDate { get; set; }


        public string sex { get; set; }


        public decimal weightKg { get; set; }
    }
    public record UpdatePetDto
    {

        public Guid ownerId { get; set; }


        public string name { get; set; }


        public string species { get; set; }


        public string breed { get; set; }

        public DateTime birthDate { get; set; }

        public string sex { get; set; }

        [Required]
        public decimal weightKg { get; set; }
    }
}
