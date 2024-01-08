using JogoVelha.Application.Extensions;
using JogoVelha.Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.AddServices();
builder.ConfigureSwagger();
builder.ConfigureApiVersioning();
builder.ConfigureAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/hub/chat");
app.ConfigureMiddlewares();
app.ConfigureCors();

app.Run();

