using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class MarshalGuardConfiguration : IEntityTypeConfiguration<MarshalGuard>
{
    public void Configure(EntityTypeBuilder<MarshalGuard> builder)
    {
        builder.HasKey(marshalGuard => marshalGuard.Id);
        builder.Property(marshalGuard => marshalGuard.Id).ValueGeneratedNever();

        builder.Property(marshalGuard => marshalGuard.EventDate).IsRequired();
        builder.Property(marshalGuard => marshalGuard.Level).IsRequired();
        builder.Property(marshalGuard => marshalGuard.RewardPhase).IsRequired();
        builder.Property(marshalGuard => marshalGuard.AllianceSize).IsRequired();
        builder.Property(marshalGuard => marshalGuard.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(marshalGuard => marshalGuard.ModifiedOn).IsRequired(false);
        builder.Property(marshalGuard => marshalGuard.ModifiedBy).IsRequired(false).HasMaxLength(150);

        builder.HasOne(marshalGuard => marshalGuard.Alliance)
            .WithMany(alliance => alliance.MarshalGuards)
            .HasForeignKey(marshalGuard => marshalGuard.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}