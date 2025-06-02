using FarmSystem.Core.Models;
using FarmSystem.Core.Interfaces;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

    }
}
