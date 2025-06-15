using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FarmSystem.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<string?> GetFilledTemplateAsync(string templateName, Dictionary<string, string> values);
        Task<string?> GetSubjectAsync(string templateName);
    }
}
