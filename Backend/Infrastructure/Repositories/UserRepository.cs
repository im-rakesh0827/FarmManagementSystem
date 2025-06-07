using FarmSystem.Core.Models;
using FarmSystem.Core.Interfaces;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace FarmSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection")!;
        }

     //    public async Task<int> RegisterAsync(User user)
     //    {
     //        using var connection = new SqlConnection(_connectionString);
     //        var sql = "INSERT INTO Users (FullName, Email, PasswordHash, Role) VALUES (@FullName, @Email, @PasswordHash, @Role); SELECT SCOPE_IDENTITY();";
     //        return await connection.ExecuteScalarAsync<int>(sql, user);
     //    }

     public async Task<int> RegisterAsync(User user)
{
    using var connection = new SqlConnection(_connectionString);

    var parameters = new DynamicParameters();
    parameters.Add("@FullName", user.FullName);
    parameters.Add("@Email", user.Email);
    parameters.Add("@PasswordHash", user.PasswordHash);
    parameters.Add("@Role", user.Role);
    parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

    await connection.ExecuteAsync("IT_InsertUpdateUser", parameters, commandType: CommandType.StoredProcedure);

    return parameters.Get<int>("@UserId");
}


public async Task<IEnumerable<User>> GetAllUsersAsync()
{
    using var connection = new SqlConnection(_connectionString);
    var procedureName = "IT_GetAllUsers";
    return await connection.QueryAsync<User>(
        procedureName,
        commandType: CommandType.StoredProcedure
    );
}


        // public async Task<User?> GetByEmailAsync(string email)
        // {
        //     using var connection = new SqlConnection(_connectionString);
        //     var sql = "SELECT * FROM Users WHERE Email = @Email";
        //     return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        // }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email); 
            var procedureName = "IT_GetUserByEmail"; 
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure);
            return user;
        }




public async Task<bool> SaveResetTokenAsync(string email, string token, DateTime expiry)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = @"UPDATE Users SET ResetToken = @Token, TokenExpiry = @Expiry WHERE Email = @Email";
    var affected = await connection.ExecuteAsync(sql, new { Email = email, Token = token, Expiry = expiry });
    return affected > 0;
}

public async Task<bool> VerifyResetTokenAsync(string email, string token)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = @"SELECT COUNT(1) FROM Users WHERE Email = @Email AND ResetToken = @Token AND TokenExpiry > GETDATE()";
    var isValid = await connection.ExecuteScalarAsync<bool>(sql, new { Email = email, Token = token });
    return isValid;
}

public async Task<bool> UpdatePasswordAsync(string email, string hashedPassword)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = @"UPDATE Users SET PasswordHash = @Password, ResetToken = NULL, TokenExpiry = NULL WHERE Email = @Email";
    var affected = await connection.ExecuteAsync(sql, new { Email = email, Password = hashedPassword });
    return affected > 0;
}





public async Task<bool> GenerateOtpAsync(string email, string otp, DateTime expiry)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = "UPDATE Users SET OtpCode = @Otp, OtpExpiry = @Expiry WHERE Email = @Email";
    var rows = await connection.ExecuteAsync(sql, new { Otp = otp, Expiry = expiry, Email = email });
    return rows > 0;
}

public async Task<bool> VerifyOtpAsync(string email, string otp)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND OtpCode = @Otp AND OtpExpiry > GETDATE()";
    var result = await connection.ExecuteScalarAsync<int>(sql, new { Email = email, Otp = otp });
    return result > 0;
}

public async Task<bool> UpdatePasswordWithOtpAsync(string email, string newPassword)
{
    using var connection = new SqlConnection(_connectionString);
    var sql = @"UPDATE Users 
                SET PasswordHash = @Password, OtpCode = NULL, OtpExpiry = NULL 
                WHERE Email = @Email";
    var rows = await connection.ExecuteAsync(sql, new { Password = newPassword, Email = email });
    return rows > 0;
}


        


    }
}
