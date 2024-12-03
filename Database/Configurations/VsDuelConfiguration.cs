using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class VsDuelConfiguration : IEntityTypeConfiguration<VsDuel>
{
    public void Configure(EntityTypeBuilder<VsDuel> builder)
    {
        builder.HasKey(vsDuel => vsDuel.Id);
        builder.Property(vsDuel => vsDuel.Id).ValueGeneratedNever();

        builder.Property(vsDuel => vsDuel.EventDate).IsRequired();
        builder.Property(vsDuel => vsDuel.Won).IsRequired();
        builder.Property(vsDuel => vsDuel.IsInProgress).IsRequired();
        builder.Property(vsDuel => vsDuel.OpponentName).IsRequired().HasMaxLength(150);
        builder.Property(vsDuel => vsDuel.OpponentServer).IsRequired();
        builder.Property(vsDuel => vsDuel.OpponentPower).IsRequired();
        builder.Property(vsDuel => vsDuel.OpponentSize).IsRequired();
        builder.Property(vsDuel => vsDuel.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(vsDuel => vsDuel.ModifiedOn).IsRequired(false);
        builder.Property(vsDuel => vsDuel.ModifiedBy).IsRequired(false).HasMaxLength(150);

        builder.HasOne(vsDuel => vsDuel.Alliance)
            .WithMany(alliance => alliance.VsDuels)
            .HasForeignKey(vsDuel => vsDuel.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vsDuel => vsDuel.VsDuelLeague)
            .WithMany(vsDuelLeague => vsDuelLeague.VsDuels)
            .HasForeignKey(vsDuel => vsDuel.VsDuelLeagueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}