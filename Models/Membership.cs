using System;

public class Membership
{
    public int Id { get; set; }              // ← entero
    public int MemberId { get; set; }        // ← entero
    public string Plan { get; set; } = "basic";      // basic | pro | premium
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "active";   // active | expired | canceled
}
