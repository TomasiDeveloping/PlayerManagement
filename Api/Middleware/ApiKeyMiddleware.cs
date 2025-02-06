using System.Text.Json;
using Api.Helpers;
using Application.Interfaces;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, IEncryptionService encryptionService)
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string AllianzIdQueryParamName = "AllianceId";
    private const string ApiKeyQueryParamName = "Key";
    public async Task InvokeAsync(HttpContext context)
    {

        if (context.User.Identity?.IsAuthenticated == true)
        {
            await next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        var allowApiKeyAttribute = endpoint?.Metadata.GetMetadata<AllowApiKeyAttribute>();

        if (allowApiKeyAttribute is null)
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.ContainsKey(ApiKeyHeaderName) && !context.Request.Query.ContainsKey(ApiKeyQueryParamName))
        {
            await WriteProblemDetailsResponse(context, StatusCodes.Status401Unauthorized, "Unauthorized", "API Key is required.");
            return;
        }

        if (!context.Request.Query.ContainsKey(AllianzIdQueryParamName))
        {
            await WriteProblemDetailsResponse(context, StatusCodes.Status400BadRequest, "Bad Request", "AllianceId is required.");
            return;
        }

        if (!Guid.TryParse(context.Request.Query[AllianzIdQueryParamName], out var allianceId))
        {
            await WriteProblemDetailsResponse(context, StatusCodes.Status400BadRequest, "Bad Request", "The provided AllianceId is not a valid GUID.");
            return;
        }



        var apiKey = context.Request.Headers.ContainsKey(ApiKeyHeaderName)
            ? context.Request.Headers[ApiKeyHeaderName]
            : context.Request.Query[ApiKeyQueryParamName];

        using var scope = scopeFactory.CreateScope();
        var apiKeyRepository = scope.ServiceProvider.GetRequiredService<IApiKeyRepository>();
        var apiKeyResult = await apiKeyRepository.GetAllianceApiKeyAsync(allianceId);

        if (apiKeyResult.IsFailure)
        {
            await WriteProblemDetailsResponse(context, StatusCodes.Status401Unauthorized, "Unauthorized", "The provided API Key is not valid.");
            return;
        }

        if (!await IsValidApiKeyAsync(apiKeyResult.Value, apiKey!))
        {
            await WriteProblemDetailsResponse(context, StatusCodes.Status401Unauthorized, "Unauthorized", "The API Key does not match.");
            return;
        }

        await next(context);
    }

    private async Task<bool> IsValidApiKeyAsync(ApiKey apiKey, string key)
    {
        var decryptedKey = await encryptionService.Decrypt(apiKey.EncryptedKey).ConfigureAwait(false);

        return key == decryptedKey;
    }

    private static async Task WriteProblemDetailsResponse(HttpContext context, int statusCode, string title,
        string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var type = GetErrorTypeForStatusCode(statusCode);

        var problemDetail = new ProblemDetails
        {
            Type = type,
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow.ToString("o"),
                ["method"] = context.Request.Method
            }
        };

        await context.Response.WriteAsJsonAsync(problemDetail,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    private static string GetErrorTypeForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            401 => "https://tools.ietf.org/html/rfc9110#section-15.2.2",
            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5", 
            500 => "https://tools.ietf.org/html/rfc9110#section-15.6.1", 
            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.5"  
        };
    }
}