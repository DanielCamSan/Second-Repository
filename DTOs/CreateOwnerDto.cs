using System.ComponentModel.DataAnnotations;

namespace VetClinic.Models.DTOs
{
    public class CreateOwnerDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}