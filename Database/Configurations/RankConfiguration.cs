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
                Id = new Guid(""),
                Name = "R5",
                Player = null!
            },
            new()
            {
                Id = new Guid(""),
                Name = "R4",
                Player = null!
            },
            new()
            {
                Id = new Guid(""),
                Name = "R3",
                Player = null!
            },
            new()
            {
                Id = new Guid(""),
                Name = "R2",
                Player = null!
            },
            new()
            {
                Id = new Guid(""),
                Name = "R1",
                Player = null!
            }
        };

        builder.HasData(ranks);
    }
}