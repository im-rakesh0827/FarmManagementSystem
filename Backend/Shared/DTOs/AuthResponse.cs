public class AuthResponse
{
    public string Token { get; set; }
    public string Role { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}

public class EmailDto { public string Email { get; set; } }
public class VerifyOtpDto { public string Email { get; set; } public string Otp { get; set; } }
public class ResetPasswordDto { public string Email { get; set; } public string NewPassword { get; set; } }
