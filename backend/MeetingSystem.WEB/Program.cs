using MeetingSystem.DBFactory.Database;
using MeetingSystem.WEB.Config;
using MeetingSystem.WEB.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<MeetingSystemDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnectionString")));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");
    return ConnectionMultiplexer.Connect(redisConnectionString!);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "mycorspolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod()
              .WithOrigins(builder.Configuration["CorsOrigins"] ?? "http://localhost:5173");
    });
});
builder.Register();

var app = builder.Build();

app.UseCors("mycorspolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TokenAuthMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
