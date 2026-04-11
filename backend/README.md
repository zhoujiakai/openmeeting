# Backend

ASP.NET Core 10.0 后端服务，PostgreSQL + Redis。

```
MeetingSystem.WEB        : Web API 入口
MeetingSystem.Service    : 业务逻辑
MeetingSystem.IService   : 业务接口
MeetingSystem.DBFactory  : 数据库上下文与迁移
MeetingSystem.Model      : 数据库实体
MeetingSystem.Models     : 请求/响应 DTO
MeetingSystem.Common     : 公共工具
MeetingSystem.EfCore     : EF Core 配置
```

首次运行前需先启动依赖服务（见 `../infra/`），然后执行数据库迁移：

```bash
dotnet ef database update --project MeetingSystem.DBFactory --startup-project MeetingSystem.WEB
```

```bash
dotnet restore            # 还原依赖
dotnet build              # 构建
dotnet run --project MeetingSystem.WEB  # 启动
```
