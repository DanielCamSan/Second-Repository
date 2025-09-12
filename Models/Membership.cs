using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Membership
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid MemberId { get; set; }=Guid.NewGuid();

        [Required, StringLength(50)]
        public string Plan { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(30);

        [Required]
        public string Status { get; set; }
    }
    public record CreateMembershipDto
    {

        [Required, StringLength(50)]
        public string Plan { get; set; }

        [Required]
        public string Status { get; set; }
    }
    public record UpdateDto {

        [Required]
        public Guid MemberId { get; set; }

        [Required, StringLength(50)]
        public string Plan { get; set; }

        [Required]
        public string Status { get; set; }
    }
    
}
