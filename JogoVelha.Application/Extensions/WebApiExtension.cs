using System.Text;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using JogoVelha.Application.Middlewares;
using JogoVelha.Application.Validations;
using JogoVelha.Domain.AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Infrastructure;
using JogoVelha.Service;
using JogoVelha.Service.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace JogoVelha.Application.Extensions;

public static class WebApiExtension 
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(options => options.AddProfile<AutoMapperProfile>());
        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        });

        builder.Services.AddPostgresConfiguration(builder.Configuration);
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddServiceLayerServices(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
        builder.Services.AddScoped<IValidator<UserDTO.UserRequest>, UserValidator>();
        builder.Services.AddScoped<IValidator<UserDTO.UserLogin>,  UserLoginValidator>();
        builder.Services.AddSignalR();
    }

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt => {
            opt.TokenValidationParameters = new() {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
            };
        });
    }

    public static void ConfigureApiVersioning(this WebApplicationBuilder builder)
    {
        TokenConfiguration tokenConfiguration = new();
        builder.Configuration.GetSection("Jwt").Bind(tokenConfiguration);
        builder.Services.AddSingleton(tokenConfiguration);
        var apiVersioningBuilder = builder.Services.AddApiVersioning(options => {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"), 
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver")
            );
        });

        apiVersioningBuilder.AddApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "JogoVelha.API", Version = "v1" });
        });
    }

    public static void ConfigureMiddlewares(this WebApplication app)
    {
       app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    public static void ConfigureCors(this WebApplication app)
    {
       app.UseCors(cors => cors.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
    }


}