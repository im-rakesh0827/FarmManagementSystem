// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using FarmSystem.Core.Interfaces;
// using FarmSystem.Core.Models;
// using FarmSystem.Core.Interfaces;
// using System.Net;
// using System.Net.Mail;
// using Microsoft.Extensions.Options;
// namespace FarmSystem.Infrastructure.Services
// {
//     public class EmailServices : IEmailService
//     {

//         private readonly SmtpSettings _smtpSettings;

//     public EmailService(IOptions<SmtpSettings> smtpSettings)
//     {
//         _smtpSettings = smtpSettings.Value;
//     }

//     public async Task SendEmailAsync(EmailRequest request)
//     {
//         using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
//         {
//             Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
//             EnableSsl = _smtpSettings.EnableSsl
//         };

//         var mailMessage = new MailMessage
//         {
//             From = new MailAddress(_smtpSettings.UserName),
//             Subject = request.Subject,
//             Body = request.Body,
//             IsBodyHtml = true
//         };

//         mailMessage.To.Add(request.ToEmail);

//         await smtpClient.SendMailAsync(mailMessage);
//     }
        
//     }
// }