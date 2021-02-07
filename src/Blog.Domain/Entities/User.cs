using System;

namespace Blog.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public UserProfile Profile { get; set; }
    }
}
