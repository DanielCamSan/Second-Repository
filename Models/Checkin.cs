using System.ComponentModel.DataAnnotations;
public class Checkin
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 4000)]
    public int Hour { get; set; }
}

