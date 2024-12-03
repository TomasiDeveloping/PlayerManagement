using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class VsDuelLeagueConfiguration : IEntityTypeConfiguration<VsDuelLeague>
{
    public void Configure(EntityTypeBuilder<VsDuelLeague> builder)
    {
        builder.HasKey(vsDuelLeague => vsDuelLeague.Id);
        builder.Property(vsDuelLeague => vsDuelLeague.Id).ValueGeneratedNever();

        builder.Property(vsDuelLeague => vsDuelLeague.Name).IsRequired().HasMaxLength(150);
        builder.Property(vsDuelLeague => vsDuelLeague.Code).IsRequired();

        var vsDuelLeagues = new List<VsDuelLeague>()
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Silver League",
                Code = 1
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Gold League",
                Code = 2
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Diamond League",
                Code = 3
            }
        };

        builder.HasData(vsDuelLeagues);
    }
}