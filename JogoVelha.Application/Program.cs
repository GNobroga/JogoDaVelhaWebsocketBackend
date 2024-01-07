using System.Net.Http.Headers;
using Asp.Versioning;
using JogoVelha.Application.Middlewares;
using JogoVelha.Domain.AutoMapper;
using JogoVelha.Infrastructure;
using JogoVelha.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => {
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
}).AddXmlSerializerFormatters();

// Configration for versioning
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


// Configurations 
apiVersioningBuilder.AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAutoMapper(options => options.AddProfile<AutoMapperProfile>());


builder.Services.AddSqliteConfiguration(builder.Configuration);

builder.Services.AddDbContext<SqliteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddServiceLayerServices(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();

