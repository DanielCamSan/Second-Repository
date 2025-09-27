using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    internal class Pet
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    [Required, StringLength (100)]
    public string Name { get; set; } =string .Empty;
    [Required, RegularExpression("^(dog|cat|bird|reptile|other)$")]
    public string Species { get; set; }=string .Empty;
    [Required, StringLength(100)]
    public string Breed { get; set; } = string.Empty;
    [Required]
    public DateTime Birthdate { get; set; }
    [Required, StringLength(100)]
    public string Sex { get; set; }= string.Empty;
    //porque pones validador de string aca?
    [Required]//, StringLength(100)]
    public decimal? WeightKg { get; set; }

}
public record CreatePetDto
{
    [Required]
    public Guid OwnerId { get; init; }

    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, RegularExpression("^(dog|cat|bird|reptile|other)$")]
    public string Species { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Breed { get; init; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; init; }

    [Required, StringLength(100)]
    public string Sex { get; init; } = string.Empty;
    //porque pones validador de string aca?
    [Required]//, StringLength(100)]
    public decimal? WeightKg { get; init; }

}
public record UpdatePetDto
{
    [Required, StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, RegularExpression("^(dog|cat|bird|reptile|other)$")]
    public string Species { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Breed { get; init; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; init; }

    [Required, StringLength(100)]
    public string Sex { get; init; } = string.Empty;

    public decimal? WeightKg { get; init; }
}