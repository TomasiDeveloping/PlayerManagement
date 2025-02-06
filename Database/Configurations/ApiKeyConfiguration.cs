using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.HasKey(apiKey => apiKey.Id);
        builder.Property(apiKey => apiKey.Id).ValueGeneratedNever();

        builder.Property(apiKey => apiKey.EncryptedKey).IsRequired().HasMaxLength(64);
        builder.Property(apiKey => apiKey.CreatedOn).IsRequired();
        builder.Property(apiKey => apiKey.CreatedBy).IsRequired().HasMaxLength(150);
        builder.Property(apiKey => apiKey.ModifiedOn).IsRequired(false);
        builder.Property(apiKey => apiKey.ModifiedBy).IsRequired(false);

        builder.HasOne(apiKey => apiKey.Alliance)
            .WithOne(alliance => alliance.ApiKey)
            .HasForeignKey<ApiKey>(apiKey => apiKey.AllianceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}