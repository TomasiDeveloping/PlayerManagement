using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class PlayerCombatRecordConfiguration : IEntityTypeConfiguration<PlayerCombatRecord>
{
    public void Configure(EntityTypeBuilder<PlayerCombatRecord> builder)
    {
        builder.HasKey(cr => cr.Id);
        builder.Property(cr => cr.Id).ValueGeneratedNever();

        builder.Property(cr => cr.Squad1)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(cr => cr.Squad2)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(cr => cr.Squad3)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(cr => cr.Squad1Power)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.Squad2Power)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.Squad3Power)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.TotalHeroPower)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.Kills)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.RecordedAtUtc)
            .IsRequired();
    }
}