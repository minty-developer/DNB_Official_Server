using System.ComponentModel.DataAnnotations;

public class SignupRequestDTO
{
    /// <summary>
    /// 사용자의 학번
    /// </summary>
    [Required(ErrorMessage = "학번이 없습니다.")]
    [MinLength(4, ErrorMessage = "학번이 너무 짧습니다.")]
    [MaxLength(4, ErrorMessage = "학번이 너무 깁니다.")]
    public string StudentId { get; set; } = string.Empty;

    /// <summary>
    /// 사용자의 이름
    /// </summary>
    [Required(ErrorMessage = "이름이 없습니다.")]
    [MinLength(2, ErrorMessage = "이름이 너무 짧습니다.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 사용자의 학교 이메일
    /// </summary>
    [Required(ErrorMessage = "이메일이 입력되지 않았습니다.")]
    [EmailAddress(ErrorMessage = "유효한 이메일 형식이 아닙니다.")]
    [RegularExpression(@"^[^@\s]+@dsm\.hs\.kr$", ErrorMessage = "학교 이메일(@dsm.hs.kr)만 사용할 수 있습니다.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 사용자의 이메일 인증 코드
    /// </summary>
    [Required(ErrorMessage = "인증 코드가 없습니다.")]
    [MinLength(4, ErrorMessage = "인증코드가 너무 짧습니다.")]
    [MaxLength(4, ErrorMessage = "인증코드가 너무 깁니다.")]
    public string EmailVerification { get; set; } = string.Empty;

    /// <summary>
    /// 사용자의 비밀번호
    /// </summary>
    [Required(ErrorMessage = "비밀번호가 없습니다.")]
    [MinLength(6, ErrorMessage = "비밀번호가 너무 짧습니다.")]
    [MaxLength(15, ErrorMessage = "비밀번호가 너무 깁니다.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequestDTO
{
    /// <summary>
    /// 사용자의 학교 이메일
    /// </summary>
    [Required(ErrorMessage = "이메일이 입력되지 않았습니다.")]
    [EmailAddress(ErrorMessage = "유효한 이메일 형식이 아닙니다.")]
    [RegularExpression(@"^[^@\s]+@dsm\.hs\.kr$", ErrorMessage = "학교 이메일(@dsm.hs.kr)만 사용할 수 있습니다.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 사용자의 비밀번호
    /// </summary>
    [Required(ErrorMessage = "비밀번호가 없습니다.")]
    public string Password { get; set; } = string.Empty;
}