using DNB_SERVER.Program;

public class HttpOriginCheckMiddleware(RequestDelegate next, ILogger<HttpOriginCheckMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<HttpOriginCheckMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        if(!context.Request.Headers.TryGetValue(Variable.RequireKey, out var RequiredBy)) {
            _logger.LogWarning("Request Failed |  [Missing header X-Require-By] {Method} {Path}{QueryString}", context.Request.Method, context.Request.Path, context.Request.QueryString);
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync("{\"error\": \"Missing required 'X-Require-By' header.\"}");
            return;
        } else if(RequiredBy.ToString() != Variable.RequireValue)
        {
            _logger.LogWarning("Request Failed |  [Wrong header X-Require-By] {Method} {Path}{QueryString}", context.Request.Method, context.Request.Path, context.Request.QueryString);
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync("{\"error\": \"Wrong required 'X-Require-By' header.\"}");
            return;
        }
        await _next(context);
    }
}