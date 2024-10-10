using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.PlayerName).IsRequired().HasMaxLength(200);

        builder.HasOne(user => user.Alliance)
            .WithMany(alliance => alliance.Users)
            .HasForeignKey(user => user.AllianceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}