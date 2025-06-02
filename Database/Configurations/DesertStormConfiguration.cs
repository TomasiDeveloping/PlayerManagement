using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class DesertStormConfiguration : IEntityTypeConfiguration<DesertStorm>
{
    public void Configure(EntityTypeBuilder<DesertStorm> builder)
    {
        builder.HasKey(desertStorm => desertStorm.Id);
        builder.Property(desertStorm => desertStorm.Id).ValueGeneratedNever();

        builder.Property(desertStorm => desertStorm.EventDate).IsRequired();
        builder.Property(desertStorm => desertStorm.OpponentServer).IsRequired();
        builder.Property(desertStorm => desertStorm.Won).IsRequired();
        builder.Property(desertStorm => desertStorm.IsInProgress).IsRequired();
        builder.Property(desertStorm => desertStorm.OpposingParticipants).IsRequired();
        builder.Property(desertStorm => desertStorm.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(desertStorm => desertStorm.OpponentName).IsRequired().HasMaxLength(150);
        builder.Property(desertStorm => desertStorm.ModifiedBy).IsRequired(false).HasMaxLength(150);
        builder.Property(desertStorm => desertStorm.Team).IsRequired().HasMaxLength(1).HasDefaultValue("A");
        builder.Property(desertStorm => desertStorm.ModifiedOn).IsRequired(false);

        builder.HasOne(desertStorm => desertStorm.Alliance)
            .WithMany(alliance => alliance.DesertStorms)
            .HasForeignKey(desertStorm => desertStorm.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}