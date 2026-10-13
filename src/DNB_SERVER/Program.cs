using System.Text;
using System.Text.Json.Serialization;
using DNB_SERVER.Program;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

// ================================== Build ==================================

var builder = WebApplication.CreateBuilder(args);

// Cache 사용 등록
builder.Services.AddMemoryCache();

// DB 등록
var dbType = builder.Configuration["DatabaseType"] ?? "Sqlite";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DnbDbContext>(options =>
{
    switch (dbType.ToLower())
    {
        case "mysql":
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            break;
            
        case "postgre":
        case "postgresql":
            options.UseNpgsql(connectionString);
            break;
            
        case "sqlite":
        default:
            options.UseSqlite(connectionString);
            break;
    }
});

// API 컨트롤러 서비스 등록
builder.Services.AddControllers();

// CORS 옵션들 등록
builder.Services.AddCors(options =>
{
    options.AddPolicy($"{CORSOptions.AcceptAll}", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
    options.AddPolicy($"{CORSOptions.AcceptGithub}", policy =>
    {
        policy
            .WithOrigins("https://github.com", "https://*.github.io")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Swagger 등록
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT 인증
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "dnb-server", // 토큰 발행자
            ValidAudience = "dnb-client", // 토큰 사용자 (Electron 앱 등)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Variable.JWTSecritKey))
        };
    });

// JWT 인증 매니져 등록
builder.Services.AddScoped<JwtTokenManager>();

// Role 활성화
builder.Services.AddAuthorization();

// Json 통신 규칙 정의
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Camel Case 활성화
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;

        if(Variable.DoEnumAsString) {
            // Enum을 문자로 통신
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        // 순환참조 무시
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ================================== App ==================================

var app = builder.Build();

// 초과 요청 429 미들웨어 사용 설정
if(Variable.CheckRateLimit) app.UseMiddleware<RateLimitMiddleware>();

// Origin Check 미들웨어 사용 설정
if(Variable.CheckRequireBy) app.UseMiddleware<HttpOriginCheckMiddleware>();

// 내부 Error Logging 미들웨어 사용 설정
app.UseMiddleware<ServerErrorLoggingMiddleware>();

// Http Error Logging 미들웨어 사용 설정
app.UseMiddleware<HttpErrorLoggingMiddleware>();

// Slow Request 미들웨어 사용 설정
app.UseMiddleware<SlowRequestMiddleware>();

// CORS 검사 미들웨어 사용 설정
app.UseCors($"{Variable.CORS}");

// 인증 및 인가 미들웨어 사용 설정
app.UseAuthentication();
app.UseAuthorization();

// 기본 라우팅 설정
app.MapControllers();

// 개발 환경일 경우
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 서버 실행
app.Run();