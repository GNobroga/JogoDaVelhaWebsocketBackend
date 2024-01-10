using JogoVelha.Application.Extensions;
using JogoVelha.Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureValidationModelHandler();

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
app.UseHttpsRedirection();
app.MapHub<ChatHub>("/hub/chat");
app.MapHub<GameHub>("/hub/game");
app.ConfigureMiddlewares();
app.ConfigureCors();

app.Run();

