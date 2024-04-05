using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;

namespace WebApi.Configs;

public static class SwaggerConfig
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Silicon Web Api", Version = "v1" });
            x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Query, //vart kommer nyckeln från
                Type = SecuritySchemeType.ApiKey, //vad för typ ab algoritm
                Name = "key", //nament på parametern vi letar efter 
                Description = "Enter API-Key",
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement //måst eha nyckeln
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
