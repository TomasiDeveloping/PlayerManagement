using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class DesertStormConfiguration : IEntityTypeConfiguration<DesertStorm>
{
    public void Configure(EntityTypeBuilder<DesertStorm> builder)
    {
        builder.HasKey(desertStorm => desertStorm.Id);

        builder.Property(desertStorm => desertStorm.CalendarWeek).IsRequired();
        builder.Property(desertStorm => desertStorm.Participated).IsRequired();
        builder.Property(desertStorm => desertStorm.Registered).IsRequired();
        builder.Property(desertStorm => desertStorm.Year).IsRequired();
    }
}