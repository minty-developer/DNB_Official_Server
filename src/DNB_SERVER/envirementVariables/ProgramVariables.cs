namespace DNB_SERVER.Program;

public static class Variable {
    // CORS 설정
    public static CORSOptions CORS { get; } = CORSOptions.AcceptAll;

    // Enum 통신을 문자열로 할지 결정
    public static bool DoEnumAsString { get; } = false;

    // 요청의 헤더를 검사할지 결정
    public static bool CheckRequireBy { get; } = true;

    // 요청의 한도를 검사할지 결정
    public static bool CheckRateLimit { get; } = true;

    // 헤더를 검사할 때 확인할 Key
    public static string RequireKey { get; } = "X-Require-By";

    // 헤더를 검사할 때 확인할 Value
    public static string RequireValue { get; } = "DNB Electron Application";

    // JWT
    public static string JWTSecritKey { get; } = "DNB_Super_Secret_Key_2026_Minseung";
    public static string JWTIssuer { get; } = "dnb-server";
    public static string JWTAudience { get; } = "dnb-client";

    public static int RateLimitPerMinute { get; } = 60;
}