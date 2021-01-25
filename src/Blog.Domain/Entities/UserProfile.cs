namespace Blog.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
