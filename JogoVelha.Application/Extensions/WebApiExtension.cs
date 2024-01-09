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
using Microsoft.AspNetCore.Mvc;
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
        builder.Services.AddScoped<IValidator<ForgotAccountDTO>,  ForgotAccountValidator>();
        builder.Services.AddScoped<IValidator<ConfirmEmailDTO>,  ConfirmEmailValidator>();
        builder.Services.AddSignalR();
    }

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new() {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
            };

            options.Events = new()
            {
                OnMessageReceived = context => 
                {
                    var accessToken = context.Request.Query["access_token"];
                    context.Token = accessToken;
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void ConfigureValidationModelHandler(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options => {
            options.InvalidModelStateResponseFactory = context => {

            var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);

            return new BadRequestObjectResult(new {
                Title = "Error",
                Status = 400,
                Details = errors
            });  
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
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = "Jogo da Velha API",
                Version = "v1",
                Description = "API para cadastrar e servir como conector de websockets",
                Contact = new OpenApiContact 
                {
                    Name = "Gabriel Cardoso",
                    Url = new Uri("https://github.com/GNobroga/JogoDaVelhaWebsocketBackend")
                }
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, 
                new OpenApiSecurityScheme 
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "'Bearer' [token]"
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme 
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new List<string>() 
                   }
                }
            );
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