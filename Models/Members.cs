using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class Members
    {
        public Guid Id { get; set; }
        [Required, StringLength(128)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(128)]
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public record CreateMemberDto
    {
        [Required, StringLength(128)]
        public string Email { get; init; } = string.Empty;
        [Required, StringLength(128)]
        public string Name { get; init; } = string.Empty ;
        public bool IsActive { get; init; }
    }
    public record UpdateMemberDto
    {
        [Required, StringLength(128)]
        public string Email { get; init; } = string.Empty;
        [Required, StringLength(128)]
        public string Name { get; init; } = String.Empty;
        public bool IsActive { get; init; }
    }
}
