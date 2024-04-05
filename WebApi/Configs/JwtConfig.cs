using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Configs;

public static class JwtConfig
{
    public static void RegisterJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    //här validerar vi olika saker t.ex. en utgivare
                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"], //kan ha flera audience

                    //validera en livslängd
                    ValidateLifetime = true,

                    //vår nyckel som vi ska använda sig av
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),

                    ClockSkew = TimeSpan.Zero,
                };
            });
    }
}
