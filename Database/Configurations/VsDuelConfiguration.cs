using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class VsDuelConfiguration : IEntityTypeConfiguration<VsDuel>
{
    public void Configure(EntityTypeBuilder<VsDuel> builder)
    {
        builder.HasKey(vsDuel => vsDuel.Id);

        builder.Property(vsDuel => vsDuel.Year).IsRequired();
        builder.Property(vsDuel => vsDuel.WeeklyPoints).IsRequired();
        builder.Property(vsDuel => vsDuel.CalendarWeek).IsRequired();
    }
}