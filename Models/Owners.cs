using System;
using System.ComponentModel.DataAnnotations;
namespace VetClinicApi.Models
{
    public class Owner
    {
        public Guid Id { get; set; }
        [Required, StringLength(200)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool Active { get; set; } = true;
    }
}