# OpenMeeting

会议管理 · 视频会议 · 基于 WebRTC

- **frontend** — Vue 3 + Element Plus
- **backend** — ASP.NET Core + PostgreSQL + Redis
- **infra** — Docker Compose 部署

## 快速开始

```bash
# 启动基础设施（PostgreSQL、Redis）
cd infra && docker-compose up -d

# 启动后端
cd backend && dotnet run

# 启动前端
cd frontend && npm install && npm run serve
```

- 前端：http://localhost:8086
- 后端：http://localhost:7099
- PostgreSQL：localhost:5432
- Redis：localhost:6379
