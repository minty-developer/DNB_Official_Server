using System.Diagnostics;

public class SlowRequestMiddleware(RequestDelegate next, ILogger<SlowRequestMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<SlowRequestMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();
        long elapsed = stopwatch.ElapsedMilliseconds;
        if(elapsed > 500 && elapsed < 3000) {
            _logger.LogWarning("Request Slow |  [{elapsedMillisecond}ms] {Method} {Path}{QueryString}", elapsed, context.Request.Method, context.Request.Path, context.Request.QueryString);
        } else if(elapsed >= 3000)
        {
            _logger.LogError("Request Too Slow |  [{elapsedMillisecond}ms] {Method} {Path}{QueryString}", elapsed, context.Request.Method, context.Request.Path, context.Request.QueryString);
        }
    }
}