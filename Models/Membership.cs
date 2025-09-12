using System.ComponentModel.DataAnnotations;

public class Membership
{
    public Guid id { get; init; }

    public Guid MemberId { get; init; }

    [Required, StringLength(100)]
    public string plan { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public DateTime StartTime { get; init; }

    [Required, StringLength(100)]
    public DateTime Endtime { get; init; }

    [Required, StringLength(100)]
    public string status { get; init; } = string.Empty;
}

