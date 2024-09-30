using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class MarshalGuardConfiguration : IEntityTypeConfiguration<MarshalGuard>
{
    public void Configure(EntityTypeBuilder<MarshalGuard> builder)
    {
        builder.HasKey(marshalGuard => marshalGuard.Id);

        builder.Property(marshalGuard => marshalGuard.Year).IsRequired();
        builder.Property(marshalGuard => marshalGuard.Day).IsRequired();
        builder.Property(marshalGuard => marshalGuard.Month).IsRequired();
        builder.Property(marshalGuard => marshalGuard.Participated).IsRequired();
    }
}