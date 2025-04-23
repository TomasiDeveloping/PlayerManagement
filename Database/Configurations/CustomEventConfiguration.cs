using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class CustomEventConfiguration : IEntityTypeConfiguration<CustomEvent>
{
    public void Configure(EntityTypeBuilder<CustomEvent> builder)
    {
        builder.HasKey(customEvent => customEvent.Id);
        builder.Property(customEnvent => customEnvent.Id).ValueGeneratedNever();

        builder.Property(customEvent => customEvent.Name).IsRequired().HasMaxLength(150);
        builder.Property(customEvent => customEvent.Description).IsRequired().HasMaxLength(500);
        builder.Property(customEvent => customEvent.EventDate).IsRequired();
        builder.Property(customEvent => customEvent.IsParticipationEvent).IsRequired();
        builder.Property(customEvent => customEvent.IsPointsEvent).IsRequired();
        builder.Property(customEvent => customEvent.IsInProgress).IsRequired();
        builder.Property(customEvent => customEvent.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(customEvent => customEvent.ModifiedBy).IsRequired(false).HasMaxLength(150);
        builder.Property(customEvent => customEvent.ModifiedOn).IsRequired(false);

        builder.HasOne(c => c.Alliance)
            .WithMany(a => a.CustomEvents)
            .HasForeignKey(c => c.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(customEvent => customEvent.CustomEventCategory)
            .WithMany(customEventCategory => customEventCategory.CustomEvents)
            .HasForeignKey(customEvent => customEvent.CustomEventCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}