using System.ComponentModel.DataAnnotations;

namespace FirstExam.Models
{
    public class CheckIns
    {
        public Guid Id { get; set; }
        public string BadgeCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
        
}

public record CreateCheckInsDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public DateTime Timestamp { get; set; } = DateTime.Now;

}

public record UpdateCheckInsDto
{
    [Required, StringLength(100)]
    public string BadgeCode { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public DateTime Timestamp { get; set; } = DateTime.Now;

}