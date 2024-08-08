using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Catalog.Services;

public static class OAuth2Service
{
    private const string SECURITY_SCHEMA = "OAuth2";
    
    public static IServiceCollection AddOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = bool.TryParse(configuration["OIDC:RequireHttpsMetadata"], out bool requireHttpsMetadata);
        var authority = configuration["OIDC:Authority"];

        _ = services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.Authority = authority;
                options.MetadataAddress = configuration["OIDC:MetadataAddress"] ?? throw new ArgumentNullException("OIDC:MetadataAddress");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authority,
                    ValidAudiences = [configuration["OIDC:ClientId"], "account"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["OIDC:ClientSecret"] ?? string.Empty))
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
                        AuthorizationUrl = new Uri($"{configuration["OIDC:Authority"]}/protocol/openid-connect/auth"),
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
}
