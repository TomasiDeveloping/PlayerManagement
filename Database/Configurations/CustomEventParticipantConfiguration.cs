using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class CustomEventParticipantConfiguration : IEntityTypeConfiguration<CustomEventParticipant>
{
    public void Configure(EntityTypeBuilder<CustomEventParticipant> builder)
    {
        builder.HasKey(customEventParticipant => customEventParticipant.Id);
        builder.Property(customEventParticipant => customEventParticipant.Id).ValueGeneratedNever();

        builder.Property(customEventParticipant => customEventParticipant.PlayerId).IsRequired();
        builder.Property(customEventParticipant => customEventParticipant.CustomEventId).IsRequired();
        builder.Property(customEventParticipant => customEventParticipant.Participated).IsRequired(false);
        builder.Property(customEventParticipant => customEventParticipant.AchievedPoints).IsRequired(false);

        builder.HasOne(customEventParticipant => customEventParticipant.Player)
            .WithMany(player => player.CustomEventParticipants)
            .HasForeignKey(customEventParticipant => customEventParticipant.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(customEventParticipant => customEventParticipant.CustomEvent)
            .WithMany(customEvent => customEvent.CustomEventParticipants)
            .HasForeignKey(customEventParticipant => customEventParticipant.CustomEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}