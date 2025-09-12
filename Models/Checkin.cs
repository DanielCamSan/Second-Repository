using System;
using System.ComponentModel.DataAnnotations;
public class Checkin
{
    public Guid Id { get; set; }

    public string BadgeCode { get; set; } = string.Empty;

    public  DateTime Timestap{ get; set; } 
}
