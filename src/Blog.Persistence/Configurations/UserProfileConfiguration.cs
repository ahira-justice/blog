using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistence.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .Property(profile => profile.FirstName)
                .HasMaxLength(150);
            builder
                .Property(profile => profile.LastName)
                .HasMaxLength(150);
        }
    }
}
