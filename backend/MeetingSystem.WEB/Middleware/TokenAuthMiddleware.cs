using MeetingSystem.Common.Utils;
using StackExchange.Redis;
using System.Text.Json;

namespace MeetingSystem.WEB.Middleware
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly HashSet<string> PublicPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/api/auth/login",
            "/api/auth/seeddata",
        };

        public TokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConnectionMultiplexer redis)
        {
            var path = context.Request.Path.Value;
            if (path != null && (PublicPaths.Contains(path)
                || path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new R(401, "未登录"));
                return;
            }

            var db = redis.GetDatabase();
            bool tokenValid = false;

            // Check all auth:* keys for matching token
            var serverKeys = redis.GetServer(redis.GetEndPoints()[0]).Keys(pattern: "auth:*");
            foreach (var key in serverKeys)
            {
                var storedToken = db.StringGet(key);
                if (storedToken == authHeader)
                {
                    tokenValid = true;
                    // Extract username from key "auth:username"
                    var userName = key.ToString().Substring("auth:".Length);
                    context.Items["UserName"] = userName;
                    break;
                }
            }

            if (!tokenValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new R(401, "token无效或已过期"));
                return;
            }

            await _next(context);
        }
    }
}
