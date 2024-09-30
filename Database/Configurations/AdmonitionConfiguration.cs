using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class AdmonitionConfiguration : IEntityTypeConfiguration<Admonition>
{
    public void Configure(EntityTypeBuilder<Admonition> builder)
    {
        builder.HasKey(admonition => admonition.Id);

        builder.Property(admonition => admonition.Reason).IsRequired().HasMaxLength(250);
    }
}