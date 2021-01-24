using Microsoft.EntityFrameworkCore;
using Blog.Domain.Entities;

namespace Blog.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> NationalParks { get; set; }
    }
}
