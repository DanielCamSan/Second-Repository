using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Membership
    {
    public Guid Id { get; set; }


        public Guid MemberId { get; set; }

        [Required, StringLength(100)]
        public string Plan { get; set; } = string.Empty;   // basic | pro | premium
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        [Required, StringLength(100)]
        string Status { get; set; } = string.Empty;   // active | expired | canceled


    }
}
