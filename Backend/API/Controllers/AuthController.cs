using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using FarmSystem.Core.Models;


namespace FarmSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = request.Role ?? "User"
            };

            var userId = await _userRepository.RegisterAsync(user);
            if (userId <= 0)
                return StatusCode(500, new { message = "Registration failed." });

            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
          var user = await _userRepository.GetByEmailAsync(request.Email);
          if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
          {
               return Unauthorized(new { message = "Invalid email or password." });
          }

          var token = GenerateJwtToken(user);

          return Ok(new
          {
               token,
               role = user.Role,
               fullName = user.FullName,
               email = user.Email
          });
      }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new System.Exception("JWT Secret Key is missing")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("fullName", user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, System.Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: System.DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"] ?? "60")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }



/*
[HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
{
    var user = await _userRepository.GetByEmailAsync(request.Email);
    if (user == null)
        return NotFound(new { message = "User not found" });

    var token = Guid.NewGuid().ToString();
    var expiry = DateTime.UtcNow.AddMinutes(15);

    await _userRepository.SaveResetTokenAsync(user.Email, token, expiry);

    // Log or email the token (in production: email)
    Console.WriteLine($"🔐 Reset Token: {token}");

    return Ok(new { message = "Reset token generated. Please check your email." });
}

[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
{
    var isValid = await _userRepository.VerifyResetTokenAsync(request.Email, request.Token);
    if (!isValid)
        return BadRequest(new { message = "Invalid or expired reset token." });

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
    var updated = await _userRepository.UpdatePasswordAsync(request.Email, hashedPassword);

    return updated
        ? Ok(new { message = "Password has been reset successfully." })
        : StatusCode(500, new { message = "Failed to update password." });
}
*/







[HttpPost("request-otp")]
public async Task<IActionResult> RequestOtp([FromBody] EmailDto request)
{
    if (string.IsNullOrWhiteSpace(request.Email))
        return BadRequest(new { message = "Email is required." });
    var user = await _userRepository.GetByEmailAsync(request.Email);
    if (user == null)
    {
        Console.WriteLine("User not found for email: " + request.Email);
        return NotFound(new { message = "User not found." });
    }
    var otp = new Random().Next(100000, 999999).ToString();
    var expiry = DateTime.UtcNow.AddMinutes(10);
    var success = await _userRepository.GenerateOtpAsync(request.Email, otp, expiry);

    if (!success)
        return StatusCode(500, new { message = "Failed to generate OTP. Please try again." });
    // TODO: Send OTP via email (SMTP or Email service integration)
    return Ok(new { message = "OTP sent to your email." });
}



[HttpPost("verify-otp")]
public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
{
    try
    {
        var isValid = await _userRepository.VerifyOtpAsync(dto.Email, dto.Otp);
        return isValid
            ? Ok(new { message = "OTP verified." })
            : BadRequest(new { message = "Invalid or expired OTP." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Server error. Please check logs." });
    }
}



   [HttpPost("reset-password-otp")]
public async Task<IActionResult> ResetPasswordViaOtp([FromBody] ResetPasswordDto dto)
{
    try
    {
        // Get the user
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest(new { message = "User not found." });

        // Hash the new password
        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        // Update the password
        var success = await _userRepository.UpdatePasswordAsync(dto.Email, hashed);

        return success
            ? Ok(new { message = "Password reset successful." })
            : BadRequest(new { message = "Password reset failed." });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] ResetPasswordViaOtp failed for {dto.Email}: {ex.Message}");
        return StatusCode(500, new { message = "Server error. Please check logs." });
    }
}


    }
}
