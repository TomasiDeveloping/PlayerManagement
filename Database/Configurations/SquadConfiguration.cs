using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class SquadConfiguration : IEntityTypeConfiguration<Squad>
{
    public void Configure(EntityTypeBuilder<Squad> builder)
    {
        builder.HasKey(squad => squad.Id);
        builder.Property(squad => squad.Id).ValueGeneratedNever();

        builder.Property(squad => squad.Power).IsRequired().HasPrecision(18,2);
        builder.Property(squad => squad.Position).IsRequired();
        builder.Property(squad => squad.LastUpdateAt).IsRequired();

        builder.HasOne(squad => squad.SquadType)
            .WithMany(squadType => squadType.Squads)
            .HasForeignKey(squad => squad.SquadTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(squatType => squatType.Player)
            .WithMany(player => player.Squads)
            .HasForeignKey(squad => squad.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}