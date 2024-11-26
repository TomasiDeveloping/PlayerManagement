using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class VsDuelParticipantConfiguration : IEntityTypeConfiguration<VsDuelParticipant>
{
    public void Configure(EntityTypeBuilder<VsDuelParticipant> builder)
    {
        builder.HasKey(vsDuelParticipant => vsDuelParticipant.Id);
        builder.Property(vsDuelParticipant => vsDuelParticipant.Id).ValueGeneratedNever();

        builder.Property(vsDuelParticipant => vsDuelParticipant.PlayerId).IsRequired();
        builder.Property(vsDuelParticipant => vsDuelParticipant.VsDuelId).IsRequired();
        builder.Property(vsDuelParticipant => vsDuelParticipant.WeeklyPoints).IsRequired();

        builder.HasOne(vsDuelParticipant => vsDuelParticipant.Player)
            .WithMany(player => player.VsDuelParticipants)
            .HasForeignKey(vsDuelParticipant => vsDuelParticipant.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vsDuelParticipant => vsDuelParticipant.VsDuel)
            .WithMany(vsDuel => vsDuel.VsDuelParticipants)
            .HasForeignKey(vsDuelParticipant => vsDuelParticipant.VsDuelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}