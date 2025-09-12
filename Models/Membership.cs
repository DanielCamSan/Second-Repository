using System.ComponentModel.DataAnnotations;

public class Membership
{
    public Guid id { get; init; }

    public Guid MemberId { get; init; }

    [Required, StringLength(100)]
    public string plan { get; init; } = string.Empty;

    public DateTime StartTime { get; init; }

    public DateTime Endtime { get; init; }

    [Required, StringLength(100)]
    public string status { get; init; } = string.Empty;
}


///DTOs
public record CreateMembershipDto
{
    public Guid id { get; init; }

    public Guid MemberId { get; init; }

    [Required, StringLength(100)]
    public string plan { get; init; } = string.Empty;

    public DateTime StartTime { get; init; }

    public DateTime Endtime { get; init; }

    [Required, StringLength(100)]
    public string status { get; init; } = string.Empty;
}

public record UpdateMembershipDto
{
    public Guid id { get; init; }

    public Guid MemberId { get; init; }

    [Required, StringLength(100)]
    public string plan { get; init; } = string.Empty;

    public DateTime StartTime { get; init; }

    public DateTime Endtime { get; init; }

    [Required, StringLength(100)]
    public string status { get; init; } = string.Empty;
}
