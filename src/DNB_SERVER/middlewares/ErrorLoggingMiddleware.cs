public class HttpErrorLoggingMiddleware(RequestDelegate next, ILogger<HttpErrorLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<HttpErrorLoggingMiddleware> _logger = logger;
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if(context.Response.StatusCode >= 400)
        {
            _logger.LogWarning("Request Failed |  [{StatusCode}] {Method} {Path}{QueryString}", context.Response.StatusCode.ToString(), context.Request.Method, context.Request.Path, context.Request.QueryString);
        }
    }
}