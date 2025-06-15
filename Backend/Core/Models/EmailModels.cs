using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmSystem.Core.Models
{

// public class SmtpSettings
// {
//     public string Host { get; set; } = string.Empty;
//     public int Port { get; set; }
//     public bool EnableSsl { get; set; }
//     public string UserName { get; set; } = string.Empty;
//     public string Password { get; set; } = string.Empty;
// }

// public class EmailRequest
// {
//     public string ToEmail { get; set; } = string.Empty;
//     public string Subject { get; set; } = string.Empty;
//     public string Body { get; set; } = string.Empty;
// }




public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public bool EnableSSL { get; set; }
    }



    public class EmailTemplate
{
    public int Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}



}