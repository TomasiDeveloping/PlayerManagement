using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class AllianceAccessTokenConfiguration : IEntityTypeConfiguration<AllianceAccessToken>
{
    public void Configure(EntityTypeBuilder<AllianceAccessToken> builder)
    {
        builder.HasKey(token => token.Id);
        builder.Property(token => token.Id).ValueGeneratedNever();

        builder.HasIndex(token => token.Token)
            .IsUnique();

        builder.Property(token => token.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(token => token.ExpiresAtUtc)
            .IsRequired(false);

        builder.Property(token => token.IsActive)
            .IsRequired();

        builder.Property(token => token.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(token => token.Alliance)
            .WithMany(alliance => alliance.AllianceAccessTokens)
            .HasForeignKey(token => token.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}