using DNB_SERVER.Program;
using Microsoft.Extensions.Caching.Memory;

public class RateLimitMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RateLimitMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<RateLimitMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        string cacheKey = $"ratelimit_{clientIp}";

        var requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return 0;
        });

        if (requestCount > Variable.RateLimitPerMinute)
        {
            _logger.LogWarning("Rate limit |  {Ip}", clientIp);
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("{\"error\": \"Too many requests. Please try again later.\"}");
            return;
        }

        _cache.Set(cacheKey, requestCount + 1);
        await _next(context);
    }
}