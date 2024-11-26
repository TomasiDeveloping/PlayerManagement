using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class AdmonitionConfiguration : IEntityTypeConfiguration<Admonition>
{
    public void Configure(EntityTypeBuilder<Admonition> builder)
    {
        builder.HasKey(admonition => admonition.Id);
        builder.Property(admonition => admonition.Id).ValueGeneratedNever();

        builder.Property(admonition => admonition.Reason).IsRequired().HasMaxLength(250);
        builder.Property(admonition => admonition.CreatedOn).IsRequired();
        builder.Property(admonition => admonition.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(admonition => admonition.ModifiedOn).IsRequired(false);
        builder.Property(admonition => admonition.ModifiedBy).IsRequired(false).HasMaxLength(150);
    }
}