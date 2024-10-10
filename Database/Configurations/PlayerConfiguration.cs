using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(player => player.Id);

        builder.Property(player => player.PlayerName).IsRequired().HasMaxLength(250);
        builder.Property(player => player.Level).IsRequired().HasMaxLength(3);

        builder.HasOne(player => player.Rank)
            .WithMany(rank => rank.Players)
            .HasForeignKey(player => player.RankId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(player => player.VsDuels)
            .WithOne(vsDuel => vsDuel.Player)
            .HasForeignKey(vsDuel => vsDuel.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(player => player.DesertStorms)
            .WithOne(desertStorm => desertStorm.Player)
            .HasForeignKey(desertStorm => desertStorm.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(player => player.MarshalGuards)
            .WithOne(marshalGuard => marshalGuard.Player)
            .HasForeignKey(marshalGuard => marshalGuard.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(player => player.Notes)
            .WithOne(notes => notes.Player)
            .HasForeignKey(note => note.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(player => player.Admonitions)
            .WithOne(admonitions => admonitions.Player)
            .HasForeignKey(admonition => admonition.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}