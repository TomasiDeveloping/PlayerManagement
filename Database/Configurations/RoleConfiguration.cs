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
                Id = new Guid(""),
                NormalizedName = ApplicationRoles.SystemAdministrator.ToUpper(),
                Name = ApplicationRoles.SystemAdministrator
            },
            new()
            {
                Id = new Guid(""),
                NormalizedName = ApplicationRoles.Administrator.ToUpper(),
                Name = ApplicationRoles.Administrator
            },
            new()
            {
                Id = new Guid(""),
                NormalizedName = ApplicationRoles.User.ToUpper(),
                Name = ApplicationRoles.User
            },
            new()
            {
                Id = new Guid(""),
                NormalizedName = ApplicationRoles.ReadOnly.ToUpper(),
                Name = ApplicationRoles.ReadOnly
            }
        };

        builder.HasData(roles);
    }
}