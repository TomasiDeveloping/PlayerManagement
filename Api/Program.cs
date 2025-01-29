// Configure Serilog logger

using Api.Configurations;
using Application;
using Database;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Utilities.Classes;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Player Manager API . . . ");
    var builder = WebApplication.CreateBuilder(args);

    var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "log-.txt");
    builder.Host.UseSerilog((context, _, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
    });


    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.ConfigureAndApiVersioning();
    builder.Services.ConfigureAndAddSwagger();
    builder.Services.ConfigureAndAddCors();
    builder.Services.ConfigureAndAddHealthChecks(builder.Configuration);

    builder.Services.AddOptions<EmailConfiguration>()
        .BindConfiguration("EmailSettings")
        .ValidateDataAnnotations()
        .ValidateOnStart();

    var jwtSection = builder.Configuration.GetRequiredSection("Jwt");
    builder.Services.ConfigureAndAddAuthentication(jwtSection);

    var app = builder.Build();

    app.UseStaticFiles();
    app.UseDefaultFiles();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("AllowAll");

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, e.Message);
}
finally
{
    Log.CloseAndFlush();
}

