using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class MarshalGuardParticipantConfiguration : IEntityTypeConfiguration<MarshalGuardParticipant>
{
    public void Configure(EntityTypeBuilder<MarshalGuardParticipant> builder)
    {
        builder.HasKey(marshalGuardParticipant => marshalGuardParticipant.Id);
        builder.Property(marshalGuardParticipant => marshalGuardParticipant.Id).ValueGeneratedNever();

        builder.Property(marshalGuardParticipant => marshalGuardParticipant.Participated).IsRequired();
        builder.Property(marshalGuardParticipant => marshalGuardParticipant.PlayerId).IsRequired();
        builder.Property(marshalGuardParticipant => marshalGuardParticipant.MarshalGuardId).IsRequired();

        builder.HasOne(marshalGuardParticipant => marshalGuardParticipant.MarshalGuard)
            .WithMany(marshalGuard => marshalGuard.MarshalGuardParticipants)
            .HasForeignKey(marshalGuardParticipant => marshalGuardParticipant.MarshalGuardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(marshalGuardParticipant => marshalGuardParticipant.Player)
            .WithMany(player => player.MarshalGuardParticipants)
            .HasForeignKey(marshalGuardParticipant => marshalGuardParticipant.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}