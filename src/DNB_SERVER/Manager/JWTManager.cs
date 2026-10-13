using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DNB_SERVER.Program;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenManager
{
    /// <summary>
    /// User 정보를 받아 JWT 액세스 토큰을 생성합니다.
    /// </summary>
    public string GenerateToken(User user)
    {
        var secretKey = Variable.JWTSecritKey;
        var issuer = Variable.JWTIssuer;
        var audience = Variable.JWTAudience;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.StudentId),
                new Claim(ClaimTypes.Name, user.Name)
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// JWT 토큰을 검증하고 디코딩하여 내부에 담긴 ClaimsPrincipal을 반환합니다.
    /// 유효하지 않거나 만료된 토큰이면 null을 반환합니다.
    /// </summary>
    public ClaimsPrincipal? ValidateAndDecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Variable.JWTSecritKey;
        var key = Encoding.UTF8.GetBytes(secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = Variable.JWTIssuer,
            ValidateAudience = true,
            ValidAudience = Variable.JWTAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
        catch (Exception)
        {
            return null;
        }
    }
}