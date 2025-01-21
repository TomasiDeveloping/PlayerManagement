using System.Reflection;
using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Interfaces;
using Utilities.Services;

namespace Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IAllianceRepository, AllianceRepository>();
        services.AddScoped<IAdmonitionRepository, AdmonitionRepository>();
        services.AddScoped<IDesertStormRepository, DesertStormRepository>();
        services.AddScoped<IMarshalGuardRepository, MarshalGuardRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IVsDuelRepository, VsDuelRepository>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IRankRepository, RankRepository>();
        services.AddScoped<ICustomEventRepository, CustomEventRepository>();
        services.AddScoped<ICustomEventParticipantRepository, CustomEventParticipantRepository>();
        services.AddScoped<IMarshalGuardParticipantRepository, MarshalGuardParticipantRepository>();
        services.AddScoped<IVsDuelParticipantRepository, VsDuelParticipantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDesertStormParticipantRepository, DesertStormParticipantRepository>();
        services.AddScoped<IZombieSiegeRepository, ZombieSiegeRepository>();
        services.AddScoped<IZombieSiegeParticipantRepository, ZombieSiegeParticipantRepository>();
        services.AddScoped<IVsDuelLeagueRepository, VsDuelLeagueRepository>();
        services.AddScoped<IExcelService, ExcelService>();


        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IClaimTypeService, ClaimTypeService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}