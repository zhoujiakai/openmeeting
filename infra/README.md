# Infra

依赖服务：PostgreSQL 16 (`:5432`)、Redis 7 (`:6379`)，密码见 `.env.example`。

```bash
cp .env.example .env          # 创建环境配置（按需修改密码）
docker-compose up -d          # 启动依赖服务
docker-compose --profile with-backend up -d  # 含 Backend
docker-compose down           # 停止
docker-compose down -v        # 停止并清除数据
```

Backend 通过 docker-compose 启动时，`.env` 中的密码经由环境变量覆盖 `appsettings.Docker.json`，无需修改 JSON。
