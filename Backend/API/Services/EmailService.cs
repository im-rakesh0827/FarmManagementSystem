using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using FarmSystem.Core.Interfaces;
using FarmSystem.Core.Models;

namespace FarmSystem.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailTemplateRepository _emailTemplateRepo;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, IEmailTemplateRepository emailTemplateRepo)
        {
            try
            {
                _emailSettings = emailSettings.Value;
                _logger = logger;
                _emailTemplateRepo = emailTemplateRepo;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.Host)
                {
                    Port = _emailSettings.Port,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSSL
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {toEmail}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {toEmail}. Error: {ex.Message}");
                return false;
            }
        }



        public async Task<string?> GetFilledTemplateAsync(string templateName, Dictionary<string, string> values)
    {
        var template = await _emailTemplateRepo.GetTemplateByNameAsync(templateName);
        if (template == null) return null;

        string body = template.Body;
        foreach (var pair in values)
        {
            body = body.Replace($"{{{{{pair.Key}}}}}", pair.Value);
        }

        return body;
    }

    public async Task<string?> GetSubjectAsync(string templateName)
    {
        var template = await _emailTemplateRepo.GetTemplateByNameAsync(templateName);
        return template?.Subject;
    }
        
    }
}
