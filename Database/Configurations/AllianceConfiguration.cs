using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class AllianceConfiguration : IEntityTypeConfiguration<Alliance>
{
    public void Configure(EntityTypeBuilder<Alliance> builder)
    {
        builder.HasKey(alliance => alliance.Id);

        builder.Property(alliance => alliance.Name).IsRequired().HasMaxLength(200);
        builder.Property(alliance => alliance.Abbreviation).IsRequired().HasMaxLength(5);
        builder.Property(alliance => alliance.Server).IsRequired();
    }
}