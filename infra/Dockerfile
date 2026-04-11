# Backend Dockerfile
# 多阶段构建 - ASP.NET Core 10.0

# ============================================
# 阶段 1: 构建
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 复制解决方案和项目文件
COPY ["backend/MeetingSystem.sln", "./"]
COPY ["backend/MeetingSystem.WEB/MeetingSystem.WEB.csproj", "MeetingSystem.WEB/"]
COPY ["backend/MeetingSystem.Service/MeetingSystem.Service.csproj", "MeetingSystem.Service/"]
COPY ["backend/MeetingSystem.IService/MeetingSystem.IService.csproj", "MeetingSystem.IService/"]
COPY ["backend/MeetingSystem.Model/MeetingSystem.Model.csproj", "MeetingSystem.Model/"]
COPY ["backend/MeetingSystem.Common/MeetingSystem.Common.csproj", "MeetingSystem.Common/"]
COPY ["backend/MeetingSystem.DBFactory/MeetingSystem.DBFactory.csproj", "MeetingSystem.DBFactory/"]

# 还原依赖
RUN dotnet restore "MeetingSystem.sln"

# 复制所有源代码
COPY backend/ ./backend/

# 构建并发布
WORKDIR /src/backend
RUN dotnet build "MeetingSystem.sln" -c Release -o /app/build
RUN dotnet publish "MeetingSystem.sln" -c Release -o /app/publish /p:UseAppHost=false

# ============================================
# 阶段 2: 运行时
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

EXPOSE 8080

# 从构建阶段复制发布文件
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MeetingSystem.WEB.dll"]
