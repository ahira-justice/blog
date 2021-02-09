namespace Blog.Application.Queries.UserProfile
{
    public class SearchUsersQuery
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
