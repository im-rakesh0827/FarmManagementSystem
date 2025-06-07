// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace Infrastructure.Services
// {
//     public class UserService
//     {
//         public Task SaveResetTokenAsync(string email, string token)
// {
//     // Store token in DB with expiry (or use in-memory cache for demo)
// }

// public Task<bool> VerifyResetTokenAsync(string email, string token)
// {
//     // Check token validity and expiry
// }

// public Task UpdatePasswordAsync(string email, string newPassword)
// {
//     // Hash and update password in DB
// }

// private string GenerateResetToken()
// {
//     return Guid.NewGuid().ToString();
// }

//     }
// }