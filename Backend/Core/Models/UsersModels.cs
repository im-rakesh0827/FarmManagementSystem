using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSystem.Core.Models
{


public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";

        public string? ResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }

        public string OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

    }

    
    public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}


}