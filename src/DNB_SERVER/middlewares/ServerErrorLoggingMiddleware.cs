public class ServerErrorLoggingMiddleware(RequestDelegate next, ILogger<ServerErrorLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ServerErrorLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred |  {Method} {Path}{QueryString}", context.Request.Method, context.Request.Path, context.Request.QueryString);

            // 충돌 검사
            if (!context.Response.HasStarted)
            {
                // 500 반환
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync("{\"error\": \"Internal Server Error occurred.\"}");
            }
            return;
        }
    }
}