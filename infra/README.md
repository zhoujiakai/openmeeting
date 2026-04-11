# Infra

依赖服务：PostgreSQL 16 (`:5432`)、Redis 7 (`:6379`)，密码见 `.env.example`。

```bash
docker-compose up -d          # 启动依赖服务
docker-compose --profile with-backend up -d  # 含 Backend
docker-compose down           # 停止
docker-compose down -v        # 停止并清除数据
```
