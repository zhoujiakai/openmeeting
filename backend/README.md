# Backend

ASP.NET Core 10.0 后端服务，PostgreSQL + Redis。

## 快速启动

```bash
dotnet restore            # 还原依赖
dotnet build              # 构建
dotnet run --project MeetingSystem.WEB  # 启动（自动种子数据初始化）
```

## 目录结构

### 简洁版

```
MeetingSystem.WEB        : Web API 入口（控制器、中间件、配置）
MeetingSystem.Service    : 业务逻辑实现
MeetingSystem.IService   : 业务接口定义
MeetingSystem.DBFactory  : 数据库上下文与迁移
MeetingSystem.Model      : 数据库实体 + 请求/响应 DTO
MeetingSystem.Common     : 公共工具（R、JsonHelper、RedisHelper）
```

### 详细版

```
backend/
├── MeetingSystem.sln
│
├── MeetingSystem.Common/                       # 公共工具
│   ├── Helper/
│   │   ├── JsonHelper.cs                       #   JSON 序列化工具
│   │   └── RedisHelper.cs                      #   Redis 操作工具
│   └── Utils/
│       └── R.cs                                #   统一响应封装
│
├── MeetingSystem.DBFactory/                    # 数据库上下文与迁移
│   ├── Database/
│   │   └── MeetingSystemDbContext.cs           #   EF Core DbContext
│   ├── DataSeed/                               #   种子数据
│   │   ├── DataSeeder.cs                       #   数据填充入口
│   │   ├── Groups.json
│   │   ├── MeetingInfos.json
│   │   ├── MeetingReports.json
│   │   ├── MenuItems.json
│   │   ├── Papers.json
│   │   ├── Roles.json
│   │   ├── Users.json
│   │   └── WeeklyReports.json
│   └── Migrations/                             #   数据库迁移文件
│       ├── 20260330145215_InitialPostgreSQLCreate.cs
│       ├── 20260330145215_InitialPostgreSQLCreate.Designer.cs
│       └── MeetingSystemDbContextModelSnapshot.cs
│
├── MeetingSystem.IService/                     # 业务接口定义
│   ├── Auth/
│   │   ├── IAuthService.cs                     #   认证服务接口
│   │   └── IMenuItemService.cs                 #   菜单服务接口
│   ├── Base/
│   │   ├── IBaseService.cs                     #   基础服务接口
│   │   └── IGenericService.cs                  #   泛型服务接口
│   ├── Meeting/
│   │   ├── IMeetingService.cs                  #   会议服务接口
│   │   └── IMeetingReportService.cs            #   会议报告服务接口
│   └── WeeklyReport/
│       └── IWeeklyReportService.cs             #   周报服务接口
│
├── MeetingSystem.Model/                        # 数据模型与 DTO
│   ├── Dto/                                    #   请求/响应 DTO
│   │   ├── ChangePasswordDto.cs
│   │   ├── FileDto.cs
│   │   ├── LoginDto.cs
│   │   ├── LoginParams.cs
│   │   ├── MeetingInfosDto.cs
│   │   ├── MeetingReportsDto.cs
│   │   ├── MenuParams.cs
│   │   ├── PageDto.cs
│   │   ├── PageParams.cs
│   │   └── WeeklyReportsDto.cs
│   └── Models/                                 #   数据库实体
│       ├── Auth/
│       │   ├── Groups.cs
│       │   ├── MenuItems.cs
│       │   ├── Roles.cs
│       │   └── Users.cs
│       ├── Meeting/
│       │   ├── MeetingInfos.cs
│       │   └── MeetingReports.cs
│       ├── MeetingRoom/
│       │   ├── MeetingRooms.cs
│       │   └── Participants.cs
│       └── WeeklyReport/
│           └── WeeklyReports.cs
│
├── MeetingSystem.Service/                      # 业务逻辑实现
│   ├── Auth/
│   │   ├── AuthService.cs                      #   认证服务
│   │   └── MenuItemService.cs                  #   菜单服务
│   ├── Base/
│   │   ├── BaseService.cs                      #   基础服务
│   │   └── GenericService.cs                   #   泛型服务
│   ├── Meeting/
│   │   ├── MeetingService.cs                   #   会议服务
│   │   └── MeetingReportService.cs             #   会议报告服务
│   └── WeeklyReport/
│       └── WeeklyReportService.cs              #   周报服务
│
└── MeetingSystem.WEB/                          # Web API 入口
    ├── Program.cs                              #   应用入口
    ├── appsettings.json                        #   应用配置
    ├── appsettings.Development.json            #   开发环境配置
    ├── Config/
    │   ├── AutofacModuleRegister.cs            #   Autofac DI 注册
    │   └── HostBuilderExtend.cs                #   主机构建扩展
    ├── Controllers/
    │   ├── Auth/
    │   │   ├── AuthController.cs               #   认证控制器
    │   │   └── MenuController.cs               #   菜单控制器
    │   ├── File/
    │   │   └── FileController.cs               #   文件控制器
    │   ├── Meeting/
    │   │   └── MeetingController.cs            #   会议控制器
    │   ├── MeetingReport/
    │   │   └── MeetingReportController.cs      #   会议报告控制器
    │   └── WeeklyReport/
    │       └── WeeklyReportController.cs       #   周报控制器
    ├── Middleware/
    │   └── TokenAuthMiddleware.cs               #   Token 认证中间件
    └── Properties/
        └── launchSettings.json                 #   启动配置
```
