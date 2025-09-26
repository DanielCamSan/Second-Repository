using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace FirstExam
{
    public class Owners
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
    public class CreateOwerDto
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }= string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

    }
    public class UpdateOwerDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
