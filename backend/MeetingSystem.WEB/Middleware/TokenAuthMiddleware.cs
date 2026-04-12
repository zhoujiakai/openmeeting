using MeetingSystem.Common.Utils;
using StackExchange.Redis;
using System.Text.Json;

namespace MeetingSystem.WEB.Middleware
{
    /// <summary>
    /// Token 认证中间件，用于校验请求中的 Authorization 头是否为有效的登录 Token。
    /// 通过 Redis 存储的 Token 进行验证，支持配置公开路径免认证。
    /// </summary>
    public class TokenAuthMiddleware
    {
        /// <summary>
        /// 下一个中间件的委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 不需要进行 Token 认证的公开路径集合
        /// </summary>
        private static readonly HashSet<string> PublicPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/api/auth/login",
            "/api/auth/seeddata",
        };

        /// <summary>
        /// 初始化 TokenAuthMiddleware 实例
        /// </summary>
        /// <param name="next">下一个中间件的请求委托</param>
        public TokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 处理 HTTP 请求，校验 Token 有效性。
        /// 对公开路径和 Swagger 路径直接放行，其余请求需携带有效的 Token。
        /// </summary>
        /// <param name="context">当前 HTTP 上下文</param>
        /// <param name="redis">Redis 连接复用器，用于查询存储的 Token</param>
        public async Task InvokeAsync(HttpContext context, IConnectionMultiplexer redis)
        {
            var path = context.Request.Path.Value;

            // 公开路径或 Swagger 页面不需要认证，直接放行
            if (path != null && (PublicPaths.Contains(path)
                || path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            // 从请求头中获取 Authorization 字段
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                // 未携带 Token，返回 401 未登录
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new R(401, "未登录"));
                return;
            }

            // 获取 Redis 数据库实例
            var db = redis.GetDatabase();
            bool tokenValid = false;

            // 遍历 Redis 中所有 auth:* 的键，查找匹配的 Token
            var serverKeys = redis.GetServer(redis.GetEndPoints()[0]).Keys(pattern: "auth:*");
            foreach (var key in serverKeys)
            {
                var storedToken = db.StringGet(key);
                if (storedToken == authHeader)
                {
                    tokenValid = true;
                    // 从键名 "auth:username" 中提取用户名，存入上下文供后续使用
                    var userName = key.ToString().Substring("auth:".Length);
                    context.Items["UserName"] = userName;
                    // 每次请求验证通过后刷新过期时间，保持 token 不过期
                    db.KeyExpire(key, TimeSpan.FromDays(7));
                    break;
                }
            }

            // Token 无效，返回 401
            if (!tokenValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new R(401, "token无效或已过期"));
                return;
            }

            // Token 有效，继续执行后续中间件
            await _next(context);
        }
    }
}
