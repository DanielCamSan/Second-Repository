using System;

namespace GymApi.Models
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public bool Active { get; set; }
    }
}
