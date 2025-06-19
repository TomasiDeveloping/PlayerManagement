using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Database;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{

    public DbSet<Admonition> Admonitions { get; set; }

    public DbSet<Alliance> Alliances { get; set; }

    public DbSet<DesertStorm> DesertStorms { get; set; }

    public DbSet<MarshalGuard> MarshalGuards { get; set; }

    public DbSet<Note> Notes { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Rank> Ranks { get; set; }

    public DbSet<VsDuel> VsDuels { get; set; }

    public DbSet<CustomEvent> CustomEvents { get; set; }

    public DbSet<MarshalGuardParticipant> MarshalGuardParticipants { get; set; }

    public DbSet<VsDuelParticipant> VsDuelParticipants { get; set; }

    public DbSet<CustomEventParticipant> CustomEventParticipants { get; set; }

    public DbSet<DesertStormParticipant> DesertStormParticipants { get; set; }

    public DbSet<ZombieSiege> ZombieSieges { get; set; }

    public DbSet<ZombieSiegeParticipant> ZombieSiegeParticipants { get; set; }

    public DbSet<VsDuelLeague> VsDuelLeagues { get; set; }

    public DbSet<ApiKey> ApiKeys { get; set; }

    public DbSet<CustomEventCategory> CustomEventCategories { get; set; }

    public DbSet<Squad> Squads { get; set; }

    public DbSet<SquadType> SquadTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<User>(entity => { entity.ToTable(name: "Users"); });
        builder.Entity<IdentityRole<Guid>>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable(name: "UserRoles"));
        builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable(name: "RoleClaims"));
        builder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable(name: "UserLogins"));
        builder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable(name: "UserTokens"));
        builder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable(name: "UserClaims"));

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}