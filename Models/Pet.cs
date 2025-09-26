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

    
    public DateTime Birthdate { get; set; }
    [Required, StringLength(100)]
    public string sex { get; set; }
    [Required, StringLength(100)]
    public decimal? WeigthKg { get; set; }

}

