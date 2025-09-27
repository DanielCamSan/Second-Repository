using System;
using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    //pusiste data anotations innecesarios no pedi que todos sean required!
    public class Owner
    {
        public Guid Id { get; set; }
        [Required, StringLength(200)]
        //te falto el validador de email 
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string FullName { get; set; } = string.Empty;
        [Required, StringLength(7-20)]
        public string Phone { get; set; } = string.Empty;

        //Validador de string en bool? >:c
        //[Required, StringLength(200)]
        public bool Active { get; set; } = true;
    }
    public record CreateOwnerDto
    {
         public Guid Id { get; init; }
        [Required, StringLength(200)]
        public string Email { get; init; }
        [Required, StringLength(200)]
        public string FullName { get; init; }
        [Required, StringLength(maximumLength:20,MinimumLength =7)]
        public string Phone { get; init; }
        //[Required, StringLength(200)]
        public bool Active { get; init; }
    }
    public record UpdateOwnerDto
    {
        [Required, StringLength(200)]
        public string Email { get; set; }
        [Required, StringLength(200)]
        public string FullName { get; set; }
        [Required, StringLength(7 - 20)]
        public string Phone { get; init; }
        //[Required, StringLength(200)]
        public bool Active { get; set; }
    }
};

