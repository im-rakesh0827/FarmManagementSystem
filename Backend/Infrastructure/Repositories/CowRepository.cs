using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmSystem.Core.Models;
using FarmSystem.Core.Interfaces;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace FarmSystem.Infrastructure.Repositories
{
    public class CowRepository : ICowRepository
{
    private readonly IDbConnection _db;
    public CowRepository(IConfiguration config)
    {
        _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }


    // private readonly IConfiguration _config;
    // private readonly string _connectionString;

    // public CowRepository(IConfiguration config)
    // {
    //     _config = config;
    //     _connectionString = _config.GetConnectionString("DefaultConnection")!;
    // }

    public async Task<IEnumerable<Cow>> GetAllAsync() =>
        await _db.QueryAsync<Cow>("SELECT * FROM tblCows");

    public async Task<Cow?> GetByIdAsync(int id) =>
        await _db.QuerySingleOrDefaultAsync<Cow>("SELECT * FROM tblCows WHERE Id = @Id", new { Id = id });

    public async Task AddAsync(Cow cow)
    {
        string sql = @"INSERT INTO tblCows (TagNumber, Breed, BirthDate, IsMilking)
                       VALUES (@TagNumber, @Breed, @BirthDate, @IsMilking)";
        await _db.ExecuteAsync(sql, cow);
    }

    public async Task UpdateAsync(Cow cow)
    {
        string sql = @"UPDATE tblCows SET TagNumber=@TagNumber, Breed=@Breed, 
                       BirthDate=@BirthDate, IsMilking=@IsMilking WHERE Id=@Id";
        await _db.ExecuteAsync(sql, cow);
    }

    public async Task DeleteAsync(int id) =>
        await _db.ExecuteAsync("DELETE FROM tblCows WHERE Id = @Id", new { Id = id });
}

}