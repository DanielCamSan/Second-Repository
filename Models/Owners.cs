using System;
using System.ComponentModel.DataAnnotations;
namespace FirstExam.Models
{
    public class Owner
    {
        public Guid Id { get; set; }
        //Para el email el validador no es stringlength
        [Required, StringLength(200)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public bool Active { get; set; } = true;
    }
}