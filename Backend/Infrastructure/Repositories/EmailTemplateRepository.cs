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
    public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly IDbConnection _db;

    public EmailTemplateRepository(IConfiguration config)
    {
        _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    public async Task<EmailTemplate?> GetTemplateByNameAsync(string templateName)
    {
        string sql = "SELECT * FROM tblEmailTemplates WHERE TemplateName = @TemplateName";
        return await _db.QueryFirstOrDefaultAsync<EmailTemplate>(sql, new { TemplateName = templateName });
    }
}

}