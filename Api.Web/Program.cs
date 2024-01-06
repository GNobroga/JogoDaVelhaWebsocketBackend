
using Api.Hubs;
using Api.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine("security.json"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.ApplyDbConfiguration(
    connectionString: builder.Configuration.GetConnectionString("SqliteConnection")!
);

builder.ConfigureAuthentication(
    issuer: builder.Configuration["JwtProvider:Issuer"]!, 
    secret: builder.Configuration["JwtProvider:Secret"]!
);

builder.ConfigureJwtProvider(
    issuer: builder.Configuration["JwtProvider:Issuer"]!, 
    secret: builder.Configuration["JwtProvider:Secret"]!
);

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

app.Run();

