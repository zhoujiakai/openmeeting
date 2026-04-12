# OpenMeeting

会议管理 · 视频会议 · 基于 WebRTC

- **frontend** — Vue 3 + Element Plus + Socket.IO 信令服务器
- **backend** — ASP.NET Core + PostgreSQL + Redis
- **infra** — Docker Compose 部署

## 快速开始

```bash
# 启动基础设施（PostgreSQL、Redis）
cd infra && docker-compose up -d

# 启动后端
cd backend && dotnet run

# 启动前端（同时启动 Socket.IO 信令服务器）
cd frontend && npm install && npm run serve
```

- 前端：http://localhost:8086
- Socket.IO 信令服务器：http://localhost:3001
- 后端：http://localhost:7099
- PostgreSQL：localhost:5432
- Redis：localhost:6379

## 致谢

- 后台管理模板：[vue3-element-admin](https://gitee.com/asaasa/vue3-element-admin)（by Asa）
- WebRTC 视频会议：[WebRTC视频会议系统源码](https://www.bilibili.com/video/BV1GN411f7L2)（B站 UP 主：顶级云加）
