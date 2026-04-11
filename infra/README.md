# Meeting System - Infra

Backend 依赖服务的基础设施配置。

## 依赖服务

| 服务 | 端口 | 用途 | 默认密码 |
|------|------|------|----------|
| PostgreSQL 16 | 5432 | 主数据库 | `YourStrong@Passw0rd` |
| Redis 7 | 6379 | 缓存 | `YourRedis@Passw0rd` |

## 快速启动

### 仅启动依赖服务 (推荐)

```bash
cd infra
docker-compose up -d
```

### 启动依赖服务 + Backend (容器化)

```bash
cd infra
docker-compose --profile with-backend up -d
```

### 查看状态

```bash
docker-compose ps
```

### 停止服务

```bash
docker-compose down
```

### 完全清理 (包括数据卷)

```bash
docker-compose down -v
```

## 本地开发

启动依赖服务后，修改 `repos/meetingsystem/backend/MeetingSystem.WEB/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSqlConnectionString": "Host=localhost;Port=5432;Database=meetingdb;Username=postgres;Password=YourStrong@Passw0rd",
    "RedisConnectionString": "localhost:6379,password=YourRedis@Passw0rd"
  }
}
```

## 连接信息

### PostgreSQL

- Host: `localhost` 或 `postgres` (容器内)
- Port: `5432`
- User: `postgres`
- Password: `YourStrong@Passw0rd`
- Database: `meetingdb`

### Redis

- Host: `localhost` 或 `redis` (容器内)
- Port: `6379`
- Password: `YourRedis@Passw0rd`

## 数据库初始化

首次运行需要创建数据库和迁移：

```bash
# 进入 backend 目录
cd ../repos/meetingsystem/backend

# 添加迁移 (如果还没有)
dotnet ef migrations add InitialCreate --project MeetingSystem.DBFactory --startup-project MeetingSystem.WEB

# 更新数据库
dotnet ef database update --project MeetingSystem.DBFactory --startup-project MeetingSystem.WEB
```

## 故障排查

### PostgreSQL 启动慢

PostgreSQL 首次启动需要 5-10 秒初始化，请耐心等待。

### 端口冲突

如果端口已被占用，修改 `docker-compose.yml` 中的端口映射：

```yaml
ports:
  - "5433:5432"  # 使用 5433 而不是 5432
```

### 查看日志

```bash
# 所有服务
docker-compose logs

# 特定服务
docker-compose logs postgres
docker-compose logs redis
docker-compose logs backend
```
