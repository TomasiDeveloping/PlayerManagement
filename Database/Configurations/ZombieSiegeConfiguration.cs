using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class ZombieSiegeConfiguration : IEntityTypeConfiguration<ZombieSiege>
{
    public void Configure(EntityTypeBuilder<ZombieSiege> builder)
    {
        builder.HasKey(zombieSiege => zombieSiege.Id);
        builder.Property(zombieSiege => zombieSiege.Id).ValueGeneratedNever();

        builder.Property(zombieSiege => zombieSiege.EventDate).IsRequired();
        builder.Property(zombieSiege => zombieSiege.AllianceSize).IsRequired();
        builder.Property(zombieSiege => zombieSiege.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(zombieSiege => zombieSiege.Level).IsRequired();
        builder.Property(zombieSiege => zombieSiege.ModifiedOn).IsRequired(false);
        builder.Property(zombieSiege => zombieSiege.ModifiedBy).IsRequired(false).HasMaxLength(150);

        builder.HasOne(zombieSiege => zombieSiege.Alliance)
            .WithMany(alliance => alliance.ZombieSieges)
            .HasForeignKey(zombieSiege => zombieSiege.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}