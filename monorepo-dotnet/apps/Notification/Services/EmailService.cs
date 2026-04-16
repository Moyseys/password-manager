using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Notification.Interfaces;
using Notification.Options;
using SharedDto.Messages;

namespace Notification.Services;

public class EmailService(ILogger<EmailService> logger, EmailSettingsOptions emailSettings) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly EmailSettingsOptions _emailSettings = emailSettings;

    public async Task SendEmailAsync(EmailMessage emailDto)
    {
        _logger.LogDebug("EmailMessage completo recebido: {@EmailMessage}", emailDto);
        try
        {
            var body = String.Empty;
            var templateName = emailDto.TemplateName.GetEnumMember();
            if (templateName is not null)
            {
                var templatePath = Path.Combine("Templates", templateName + ".html");
                if (!File.Exists(templatePath))
                {
                    _logger.LogError("Email template not found: {TemplatePath}", templatePath);
                    return;
                }
                var sb = new StringBuilder(await File.ReadAllTextAsync(templatePath));

                foreach (var kvp in emailDto.TemplateData)
                {
                    sb.Replace($"{{{{{kvp.Key}}}}}", kvp.Value ?? "");
                }
                body = sb.ToString();
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress("", emailDto.To));
            message.Subject = emailDto.Subject;
            message.Body = new TextPart(emailDto.IsHtml ? "html" : "plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {To}", emailDto.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {To}", emailDto.To);
            throw;
        }
    }
}