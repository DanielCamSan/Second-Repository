public class CheckIn
{
    public Guid Id { get; set; }
    public string BadgeCode { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
