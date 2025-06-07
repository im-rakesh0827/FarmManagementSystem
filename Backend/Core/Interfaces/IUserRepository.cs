using FarmSystem.Core.Models;
using System.Threading.Tasks;
namespace FarmSystem.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();


Task<bool> SaveResetTokenAsync(string email, string token, DateTime expiry);
Task<bool> VerifyResetTokenAsync(string email, string token);
Task<bool> UpdatePasswordAsync(string email, string hashedPassword);



Task<bool> GenerateOtpAsync(string email, string otp, DateTime expiry);
Task<bool> VerifyOtpAsync(string email, string otp);
Task<bool> UpdatePasswordWithOtpAsync(string email, string newPassword);



    }
}
