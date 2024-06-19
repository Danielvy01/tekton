using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Tekton.Seguridad;

/// <summary>
/// SeguridadConfiguracion
/// </summary>
[ExcludeFromCodeCoverage]
public static class SeguridadConfiguracion
{
    /// <summary>
    /// AddCustomSecurity
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddCustomSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["SecuritySettings:Issuer"],
                        ValidAudience = configuration["SecuritySettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecuritySettings:Secret"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (ctx.Request.Headers.ContainsKey("X-AUTORIZATION"))
                            {
                                var bearerToken = ctx.Request.Headers["X-AUTORIZATION"][0] ?? string.Empty;
                                var token = bearerToken.StartsWith("Bearer ") ? bearerToken[7..] : bearerToken;
                                ctx.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
    }

}
