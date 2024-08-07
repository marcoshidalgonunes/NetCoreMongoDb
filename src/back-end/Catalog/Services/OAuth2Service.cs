using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Catalog.Services;

public static class OAuth2Service
{
    private const string SECURITY_SCHEMA = "OAuth2";
    

    public static IServiceCollection AddOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !$"{configuration["Keycloak:ssl-required"]}".Equals("None", StringComparison.InvariantCultureIgnoreCase);
                options.MetadataAddress = $"http://keycloak:8080/realms/{configuration["Keycloak:realm"]}/.well-known/openid-configuration";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = $"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}",
                    ValidAudiences = [$"{configuration["Keycloak:resource"]}", "account"]
                };
            });

        _ = services.AddAuthorizationBuilder()
            .AddPolicy("Default", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

        return services;
    }

    public static IServiceCollection AddSwaggerOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net Core Web API v1.0", Version = "v1" });
            c.AddSecurityDefinition(SECURITY_SCHEMA, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/auth"),

                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = SECURITY_SCHEMA //The name of the previously defined security scheme.
						}
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    private static string? GetIssuer(string metadataAddress)
    {
        using var client = new HttpClient();

        var response = client.GetAsync(metadataAddress).Result;
        response.EnsureSuccessStatusCode();

        var json = response.Content.ReadAsStringAsync().Result;
        var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        return metadata?["issuer"]?.ToString();
    }
}
