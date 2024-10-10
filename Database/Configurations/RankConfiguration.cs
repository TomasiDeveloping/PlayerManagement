using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class RankConfiguration : IEntityTypeConfiguration<Rank>
{
    public void Configure(EntityTypeBuilder<Rank> builder)
    {
        builder.HasKey(rank => rank.Id);

        builder.Property(rank => rank.Name).IsRequired().HasMaxLength(2);

        var ranks = new List<Rank>()
        {
            new()
            {
                Id = new Guid("b1c10a1c-5cf3-4e22-9fc1-d9b165b85dd3"),
                Name = "R5"
            },
            new()
            {
                Id = new Guid("0fc2f68a-0a4d-4922-981e-c624e4c39024"),
                Name = "R4"
            },
            new()
            {
                Id = new Guid("4970e1f5-f7f5-43e8-88cc-7f8fc4075418"),
                Name = "R3"
            },
            new()
            {
                Id = new Guid("d8d0c587-f269-45ff-b13e-4631298bf0af"),
                Name = "R2"
            },
            new()
            {
                Id = new Guid("326edef0-5074-43a5-9db9-edc71221a0f7"),
                Name = "R1"
            }
        };

        builder.HasData(ranks);
    }
}