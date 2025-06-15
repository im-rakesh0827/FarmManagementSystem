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
using FarmSystem.API.Services;

namespace FarmSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;


        public AuthController(IUserRepository userRepository, IConfiguration config, ILogger<AuthController> logger, IEmailService emailService)
        {
            _userRepository = userRepository;
            _config = config;
            _logger = logger;
            _emailService = emailService;
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
            if (userId <= 0) return StatusCode(500, new { message = "Registration failed." });
            bool emailSent = await SendEmail(request);
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

[HttpGet("user/{email}")]
public async Task<IActionResult> GetProfile(string email)
{
    var user = await _userRepository.GetByEmailAsync(email);
    if (user == null) return NotFound();
    return Ok(user);
}



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
    await SendOtpEmail(request.Email, user.FullName, otp);
    // await SendOtpEmail2(request.Email, user.FullName, otp);
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




private async Task<bool> SendEmail(RegisterRequest user)
{
    try
    {
        string subject = "Welcome to FarmSystem - Registration Successful!";
        string body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f2f2f2;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: auto;
                    background: #ffffff;
                    padding: 30px;
                    border-radius: 10px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                }}
                h2 {{
                    color: #4CAF50;
                    margin-bottom: 15px;
                }}
                p {{
                    font-size: 16px;
                    color: #444;
                    line-height: 1.6;
                }}
                ul {{
                    margin: 15px 0;
                    padding-left: 20px;
                }}
                li {{
                    font-size: 15px;
                    color: #333;
                }}
                .cta-button {{
                    display: inline-block;
                    background-color: #4CAF50;
                    color: white;
                    padding: 12px 24px;
                    text-decoration: none;
                    font-weight: bold;
                    border-radius: 6px;
                    margin-top: 20px;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 12px;
                    color: #888;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Hello {user.FullName}, Welcome to FarmSystem!</h2>
                <p>Thank you for registering with <strong>FarmSystem</strong> — your all-in-one solution for managing dairy and agricultural operations.</p>
                <p>Your account has been successfully created. Below are your registration details:</p>
                <ul>
                    <li><strong>Full Name:</strong> {user.FullName}</li>
                    <li><strong>Email:</strong> {user.Email}</li>
                    <li><strong>Registered Role:</strong> {user.Role}</li> <li><strong>Registered
                </ul>
                <p>You can now log in and start managing cows, milk production, inventory, employees, and more — all in one place.</p>
                <a href='https://your-farmsystem.com/login' class='cta-button'>Log In to FarmSystem</a>
                <p class='footer'>Need help? Just reply to this email or contact support. <br>– FarmSystem Team</p>
            </div>
        </body>
        </html>";

        bool emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
        if (!emailSent)
        {
            _logger.LogError($"Failed to send email to {user.Email}");
            return false;
        }

        _logger.LogInformation($"Email sent successfully to {user.Email}");
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exception while sending email: {ex.Message}");
        return false;
    }
}



private async Task<bool> SendOtpEmail(string recipientEmail, string fullName, string otp)
{
    try
    {
        string subject = "FarmSystem Password Reset OTP";
        string body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f8f8f8;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: auto;
                    background: #ffffff;
                    padding: 30px;
                    border-radius: 10px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                }}
                h2 {{
                    color: #ff6600;
                    margin-bottom: 20px;
                }}
                p {{
                    font-size: 16px;
                    color: #333333;
                    line-height: 1.5;
                }}
                .otp-code {{
                    display: inline-block;
                    font-size: 24px;
                    font-weight: bold;
                    background-color: #f2f2f2;
                    padding: 10px 20px;
                    border-radius: 6px;
                    color: #ff6600;
                    letter-spacing: 3px;
                    margin: 20px 0;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 12px;
                    color: #999999;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Password Reset Request - FarmSystem</h2>
                <p>Hello {fullName},</p>
                <p>We received a request to reset your password for your FarmSystem account. Use the following OTP to proceed:</p>
                <div class='otp-code'>{otp}</div>
                <p>This OTP is valid for <strong>10 minutes</strong>. Do not share it with anyone.</p>
                <p>If you didn’t request this password reset, please ignore this email or contact support.</p>
                <p class='footer'>© FarmSystem | Secure Agriculture & Dairy Management</p>
            </div>
        </body>
        </html>";

        bool emailSent = await _emailService.SendEmailAsync(recipientEmail, subject, body);
        if (!emailSent)
        {
            _logger.LogError($"Failed to send OTP to {recipientEmail}");
            return false;
        }

        _logger.LogInformation($"OTP email sent successfully to {recipientEmail}");
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exception while sending OTP email: {ex.Message}");
        return false;
    }
}



private async Task<bool> SendOtpEmail2(string email, string fullName, string otp)
{
    try
    {
        var subject = await _emailService.GetSubjectAsync("OtpReset");
        var body = await _emailService.GetFilledTemplateAsync("OtpReset", new Dictionary<string, string>
        {
            { "FullName", fullName },
            { "OTP", otp }
        });

        if (body == null || subject == null)
        {
            _logger.LogError("Failed to load email template.");
            return false;
        }

        bool emailSent = await _emailService.SendEmailAsync(email, subject, body);
        if (!emailSent)
        {
            _logger.LogError($"Failed to send OTP to {email}");
            return false;
        }

        _logger.LogInformation($"OTP email sent successfully to {email}");
        return true;
        // return emailSent;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exception while sending OTP: {ex.Message}");
        return false;
    }
}




    }
}
