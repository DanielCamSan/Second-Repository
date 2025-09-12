
using System.ComponentModel.DataAnnotations;

    public class Membership
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string Plan { get; set; } = string.Empty;         // basic | pro | premium
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;      // active | expired | canceled
    }

    public record CreateMembershipDto
    {
        [Required(ErrorMessage = "MemberId is required")]
        public Guid MemberId { get; init; }

        [Required(ErrorMessage = "Plan is required")]
        [RegularExpression("basic|pro|premium", ErrorMessage = "Plan must be: basic, pro, or premium")]
        public string Plan { get; init; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; init; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; init; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("active|expired|canceled", ErrorMessage = "Status must be: active, expired, or canceled")]
        public string Status { get; init; } = "active";
    }

    public record UpdateMembershipDto
    {
        [Required(ErrorMessage = "MemberId is required")]
        public Guid MemberId { get; init; }

        [Required(ErrorMessage = "Plan is required")]
        [RegularExpression("basic|pro|premium", ErrorMessage = "Plan must be: basic, pro, or premium")]
        public string Plan { get; init; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; init; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; init; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("active|expired|canceled", ErrorMessage = "Status must be: active, expired, or canceled")]
        public string Status { get; init; }
    }



