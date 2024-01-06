using System.Text;
using Api.Data.Context;
using Api.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Web.Extensions;

public static class GlobalExtension 
{
    public static WebApplicationBuilder ApplyDbConfiguration(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddDbContext<SqliteContext>(options => options.UseSqlite(connectionString));
        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder, string issuer, string secret)
    {
        builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt => {
            opt.TokenValidationParameters = new() {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };

            opt.Events = new()
            {
                OnMessageReceived = handler =>
                {
                    if (handler.Request.Path.StartsWithSegments("/hub/chat"))
                        handler.Token = handler.Request.Query["access_token"];
                    return Task.CompletedTask;
                }
            };
        });
        return builder;
    }

    public static WebApplicationBuilder ConfigureJwtProvider(this WebApplicationBuilder builder, string issuer, string secret)
    {
        var jwtProvider = new JwtProvider { Issuer = issuer, Secret = secret };
        builder.Services.AddSingleton(jwtProvider);
        return builder;
    }
}