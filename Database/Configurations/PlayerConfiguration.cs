using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(player => player.Id);
        builder.Property(player => player.Id).ValueGeneratedNever();

        builder.Property(player => player.PlayerName).IsRequired().HasMaxLength(250);
        builder.Property(player => player.Level).IsRequired();
        builder.Property(player => player.CreatedOn).IsRequired();
        builder.Property(player => player.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(player => player.ModifiedOn).IsRequired(false);
        builder.Property(player => player.ModifiedBy).IsRequired(false).HasMaxLength(150);

        builder.HasOne(player => player.Alliance)
            .WithMany(alliance => alliance.Players)
            .HasForeignKey(player => player.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(player => player.Rank)
            .WithMany(rank => rank.Players)
            .HasForeignKey(player => player.RankId)
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