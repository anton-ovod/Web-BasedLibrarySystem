using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(string.Empty, recipientEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = message
            };

            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await smtpClient.SendAsync(email);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
