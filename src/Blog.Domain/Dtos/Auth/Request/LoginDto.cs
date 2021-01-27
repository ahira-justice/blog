namespace Blog.Domain.Dtos.Auth.Request
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int? ExpiresAt { get; set; }

    }
}
