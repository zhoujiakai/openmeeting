# Infra

依赖服务：PostgreSQL 16 (`:5432`)、Redis 7 (`:6379`)、pgAdmin 4 (`:5050`)，密码见 `.env.example`。

## 快速开始

```bash
cp .env.example .env          # 创建环境配置（按需修改密码）
docker-compose up -d          # 启动依赖服务
docker-compose --profile with-backend up -d  # 含 Backend
docker-compose down           # 停止
docker-compose down -v        # 停止并清除数据
```

- 查看数据库：浏览器访问 http://localhost:5050 ，登录账密 `admin@admin.com` / `admin123`。添加服务器时 Host 填 `postgres`，其余连接信息同`.env`

## 文件目录

```
.env.example          : 环境变量模板
docker-compose.yml    : 服务编排（PostgreSQL、Redis、pgAdmin、Backend）
Dockerfile            : Backend 多阶段构建
```
