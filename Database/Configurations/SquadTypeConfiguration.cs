using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class SquadTypeConfiguration : IEntityTypeConfiguration<SquadType>
{
    public void Configure(EntityTypeBuilder<SquadType> builder)
    {
        builder.HasKey(squadType => squadType.Id);
        builder.Property(squadType => squadType.Id).ValueGeneratedNever();

        builder.Property(squadType => squadType.TypeName)
            .IsRequired()
            .HasMaxLength(150);

        var squadTypes = new List<SquadType>()
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                TypeName = "Tanks"
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                TypeName = "Missile"
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                TypeName = "Aircraft"
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                TypeName = "Mixed"
            }
        };

        builder.HasData(squadTypes);


    }
}