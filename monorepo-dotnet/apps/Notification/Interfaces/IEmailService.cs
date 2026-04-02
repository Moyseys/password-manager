using SharedDto.Messages;

namespace Notification.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailDto);
}