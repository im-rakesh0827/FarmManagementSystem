using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmSystem.Core.Models;

namespace FarmSystem.Core.Interfaces
{
    public interface IEmailTemplateRepository
{
    Task<EmailTemplate?> GetTemplateByNameAsync(string templateName);
}
}