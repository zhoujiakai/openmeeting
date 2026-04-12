# OpenMeeting

会议管理 · 视频会议 · 基于 WebRTC

- **frontend** — Vue 3 + Element Plus + Socket.IO 信令服务器
- **backend** — ASP.NET Core + PostgreSQL + Redis
- **infra** — Docker Compose 部署

## 环境要求

- [Docker](https://www.docker.com/) — 运行 PostgreSQL、Redis 等基础设施
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) — 后端开发与运行
- [Node.js](https://nodejs.org/) ≥ 16 — 前端开发与运行（含 npm）

## 快速开始

```bash
# 启动基础设施（PostgreSQL、Redis）
cd infra && docker-compose up -d

# 启动后端
cd backend && dotnet run

# 启动前端（同时启动 Socket.IO 信令服务器）
cd frontend && npm install && npm run serve
```

## 本地访问

- 前端：http://localhost:8086
- Socket.IO 信令服务器：http://localhost:3001
- 后端：http://localhost:7099
- PostgreSQL：localhost:5432
- Redis：localhost:6379
- pgAdmin（数据库可视化管理）：http://localhost:5050，登录账密 `admin@admin.com` / `admin123`。添加服务器时 Host 填 `postgres`，其余连接信息同 `infra/.env`

## 致谢

- 后台管理模板：[vue3-element-admin](https://gitee.com/asaasa/vue3-element-admin)（by Asa）
- WebRTC 视频会议：[WebRTC视频会议系统源码](https://www.bilibili.com/video/BV1GN411f7L2)（B站 UP 主：顶级云加）
