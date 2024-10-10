using System.Reflection;
using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddTransient<IJwtService, JwtService>();

        return services;
    }
}