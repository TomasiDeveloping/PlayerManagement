using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class CustomEventCategoryConfiguration : IEntityTypeConfiguration<CustomEventCategory>
{
    public void Configure(EntityTypeBuilder<CustomEventCategory> builder)
    {
        builder.HasKey(customEventCategory => customEventCategory.Id);
        builder.Property(customEventCategory => customEventCategory.Id).ValueGeneratedNever();

        builder.Property(customEventCategory => customEventCategory.AllianceId).IsRequired();
        builder.Property(customEventCategory => customEventCategory.Name).IsRequired().HasMaxLength(255);
        builder.Property(customEventCategory => customEventCategory.IsPointsEvent).IsRequired();
        builder.Property(customEventCategory => customEventCategory.IsParticipationEvent).IsRequired();

        builder.HasOne(customEventCategory => customEventCategory.Alliance)
            .WithMany(alliance => alliance.CustomEventCategories)
            .HasForeignKey(customEventCategory => customEventCategory.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}