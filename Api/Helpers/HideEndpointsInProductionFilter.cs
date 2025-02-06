using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Helpers;

public class HideEndpointsInProductionFilter(IWebHostEnvironment environment) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (environment.IsProduction())
        {
            var allowedPaths = new List<string>
            {
                "/api/v{version}/Players/Mvp"
            };

            var allowedSchemas = new List<string>
            {
                "PlayerMvpDto"
            };

            var pathsToRemove = swaggerDoc.Paths
                .Where(path => !allowedPaths.Any(allowedPath => path.Key.Contains(allowedPath)))
                .ToList();

            foreach (var path in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(path.Key);
            }

            var schemaToRemove = context.SchemaRepository.Schemas
                .Where(schema => !allowedSchemas.Contains(schema.Key))
                .ToList();

            foreach (var schema in schemaToRemove)
            {
                context.SchemaRepository.Schemas.Remove(schema.Key);
            }
        }
    }
}