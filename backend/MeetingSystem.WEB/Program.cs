using MeetingSystem.DBFactory.DataSeed;
using MeetingSystem.DBFactory.Database;
using MeetingSystem.WEB.Config;
using MeetingSystem.WEB.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

// 创建 Web 应用构建器
var builder = WebApplication.CreateBuilder(args);

// 注册控制器服务，并使用 Newtonsoft.Json 作为 JSON 序列化器
builder.Services.AddControllers().AddNewtonsoftJson();
// 注册 API 端点探索服务（用于 Swagger 等）
builder.Services.AddEndpointsApiExplorer();
// 注册 Swagger 文档生成服务
builder.Services.AddSwaggerGen();
// 注册 PostgreSQL 数据库上下文，使用连接池提升性能
builder.Services.AddDbContextPool<MeetingSystemDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnectionString")));
// 注册 Redis 连接单例，用于缓存和 Token 管理
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");
    return ConnectionMultiplexer.Connect(redisConnectionString!);
});
// 配置跨域策略，允许指定来源的前端访问
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "mycorspolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod()
              .WithOrigins(builder.Configuration["CorsOrigins"] ?? "http://localhost:5173");
    });
});
// 注册 Autofac 容器及模块
builder.Register();

// 构建 Web 应用程序
var app = builder.Build();

// 自动执行种子数据初始化
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MeetingSystemDbContext>();
    await DataSeeder.SeedAsync(db);
}

// 启用跨域中间件
app.UseCors("mycorspolicy");

// 开发环境下启用 Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 启用 HTTPS 重定向
app.UseHttpsRedirection();
// 启用自定义 Token 认证中间件
app.UseMiddleware<TokenAuthMiddleware>();
// 启用授权中间件
app.UseAuthorization();
// 映射控制器路由
app.MapControllers();
// 启动应用程序
app.Run();
