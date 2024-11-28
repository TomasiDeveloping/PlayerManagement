using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class ZombieSiegeParticipantConfiguration : IEntityTypeConfiguration<ZombieSiegeParticipant>
{
    public void Configure(EntityTypeBuilder<ZombieSiegeParticipant> builder)
    {
        builder.HasKey(zombieSiegeParticipant => zombieSiegeParticipant.Id);
        builder.Property(zombieSiegeParticipant => zombieSiegeParticipant.Id).ValueGeneratedNever();

        builder.Property(zombieSiegeParticipant => zombieSiegeParticipant.PlayerId).IsRequired();
        builder.Property(zombieSiegeParticipant => zombieSiegeParticipant.ZombieSiegeId).IsRequired();
        builder.Property(zombieSiegeParticipant => zombieSiegeParticipant.SurvivedWaves).IsRequired();

        builder.HasOne(zombieSiegeParticipant => zombieSiegeParticipant.Player)
            .WithMany(player => player.ZombieSiegeParticipants)
            .HasForeignKey(zombieSiegeParticipant => zombieSiegeParticipant.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(zombieSiegeParticipant => zombieSiegeParticipant.ZombieSiege)
            .WithMany(zombieSiege => zombieSiege.ZombieSiegeParticipants)
            .HasForeignKey(zombieSiegeParticipant => zombieSiegeParticipant.ZombieSiegeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}