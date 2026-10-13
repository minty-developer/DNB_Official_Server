using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/v1")]
public partial class AuthController(DnbDbContext dbContext) : ControllerBase
{
    private DnbDbContext _context = dbContext;
    private static readonly PasswordHasher<User> _passwordHasher = new();

    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignupRequestDTO data)
    {
        // Email Unique Check
        if(FindUser(data.Email, out _)) {
            return StatusCode(409, new { message = "User already exsists" });
        }

        // Regist
        var user = new User
        {
            Email = data.Email.ToLower(),
            StudentId = data.StudentId,
            Name = data.Name,
            Coin = 0,
        };

        // Hashing
        user.PasswordHash = _passwordHasher.HashPassword(user, data.Password);
        _context.Users.Add(user);

        // Commit
        _context.SaveChanges();

        // Success
        return StatusCode(201);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDTO data, [FromServices] JwtTokenManager jwtManager)
    {
        if(!FindUser(data.Email, out var user)
        || _passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, data.Password)
            == PasswordVerificationResult.Failed) {
            return StatusCode(401);
        }
        return StatusCode(200, new { token = jwtManager.GenerateToken(user) });
    }

    bool FindUser(string email, out User? user)
    {
        user = _context.Users.FirstOrDefault(e => e.Email == email);
        return user is not null;
    }
}