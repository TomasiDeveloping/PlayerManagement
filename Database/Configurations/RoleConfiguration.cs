using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utilities.Constants;

namespace Database.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        var roles = new List<IdentityRole<Guid>>()
        {
            new()
            {
                Id = new Guid("d8b9f882-95f0-4ba0-80ed-9c22c27ac88a"),
                NormalizedName = ApplicationRoles.SystemAdministrator.ToUpper(),
                Name = ApplicationRoles.SystemAdministrator
            },
            new()
            {
                Id = new Guid("47de05ba-ff1e-46b6-9995-269084006c24"),
                NormalizedName = ApplicationRoles.Administrator.ToUpper(),
                Name = ApplicationRoles.Administrator
            },
            new()
            {
                Id = new Guid("5cc27946-5601-4a25-b9a9-75b8a11c0cf4"),
                NormalizedName = ApplicationRoles.User.ToUpper(),
                Name = ApplicationRoles.User
            },
            new()
            {
                Id = new Guid("207bb0a3-ad50-49bb-bc41-b266fce66529"),
                NormalizedName = ApplicationRoles.ReadOnly.ToUpper(),
                Name = ApplicationRoles.ReadOnly
            }
        };

        builder.HasData(roles);
    }
}