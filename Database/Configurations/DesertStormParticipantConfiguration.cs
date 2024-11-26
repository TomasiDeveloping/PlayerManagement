using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class DesertStormParticipantConfiguration : IEntityTypeConfiguration<DesertStormParticipant>
{
    public void Configure(EntityTypeBuilder<DesertStormParticipant> builder)
    {
        builder.HasKey(desertStormParticipant => desertStormParticipant.Id);
        builder.Property(desertStormParticipant => desertStormParticipant.Id).ValueGeneratedNever();

        builder.Property(desertStormParticipant => desertStormParticipant.PlayerId).IsRequired();
        builder.Property(desertStormParticipant => desertStormParticipant.DesertStormId).IsRequired();
        builder.Property(desertStormParticipant => desertStormParticipant.Participated).IsRequired();
        builder.Property(desertStormParticipant => desertStormParticipant.Registered).IsRequired();
        builder.Property(desertStormParticipant => desertStormParticipant.StartPlayer).IsRequired();

        builder.HasOne(desertStormParticipant => desertStormParticipant.DesertStorm)
            .WithMany(desertStorm => desertStorm.DesertStormParticipants)
            .HasForeignKey(desertStormParticipant => desertStormParticipant.DesertStormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(desertStormParticipant => desertStormParticipant.Player)
            .WithMany(player => player.DesertStormParticipants)
            .HasForeignKey(desertStormParticipant => desertStormParticipant.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}